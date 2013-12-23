Markdown Processor
===============

A markdown processor written in C#. It runs on the .NET library.


Output
---------

This project can currently output to `docx` word format, which can then be converted to `pdf` format. The project has been designed to work with different outputs, so future outputs will be added. Here's some planned formats that will be supported:

1. `HTML` - this is the normal output for markdown, so this project will support this as well.
2. `LaTeX` - `LaTeX` is a very awesome format that can be styled and controlled very easily and instantly, and is used for typesetting books (mostly textbooks) and scientific papers. It is kinda complicated to use however, so having this output to `LaTeX` will get all it's typesetting benefits without having to know `LaTeX`. 
3. `PowerPoint` - divide slides based horizontal rules and headers, and build a slideshow from a markdown document. It will also support `LaTeX` slideshows as well, using a package like `beamer`.
4. `SVG` - A picture format that will leave the text perfectly crisp no matter how zoomed in it is. Other formats such as `PNG` can be created from this format if needed. (`PNG` and other picture formats may be supported directly in the future.

Progress
------------

Markdown is simple to learn, but still rather large, and there's no actual standard format. There are many extensions to Markdown, and eventually this project will support most of the common ones (such as tables and automatic hyperlinking), however the core of Markdown must be implemented first. A specification (sort of) for the core [can be found here](http://daringfireball.net/projects/markdown/syntax). The major elements are listed below along with their progress:

1. **Paragraphs** - Complete, not fully tested
+ **Line Breaks** - Complete, not fully tested
+ **Headers** - Complete, not fully tested
+ **Block Quotes** - Not started
+ **Lists** - Not started
+ **Code Blocks** - Not started
+ **Horizontal Rules** - Not started

As well the following inline elements:

1. **Bold** - Complete, not fully tested
+ **Italics** - Complete, not fully tested
+ **Links** - Partially completed (inline complete, reference not started)
+ **Inline Code** - Not started

Additional features progress: (see below for an explanation, these features are not standard in Markdown, and specific to this processor, but allow for much more flexibility in markdown, and give it significant power)

1. **Output Code Blocks**- Not started
+ **Output Language Specific Code Blocks** - Not started
+ **Scope Regions** - Not started
+ **Commands/Text Macros** - Not Started
+ **Output Classes** - Not started 

Additional Features to Markdown
---

Markdown is not really a standardized format, but there are a core set of features. This project will aim to support as much of the core set of features as possible. It will also add additional features to the format. Here are the planned features:

###Output code blocks###

In some versions of Markdown you can write html code that will simply be passed over by the formatting and included in the output. This is a useful way to allow you to still have the flexibility of a more complex format without having to use it all the time. If markdown doesn't support some feature, you can simply write the html for it.

While this is very useful, it is also very limited, and the way it's implemented is problematic. You simply write HTML code inline and the processor passes over it. We'll pass over the chance that it could be slightly confusing to write some html code and have it disappear from the output, and just consider the fact that it ties markdown too much to html. Any document that uses this inline html can't be compiled to other formats, and supporting only html means that Markdown is just an html processor, not a whole format of it's own.

The point of this project is to support other output format other than just html, so tying markdown to html is not something this project would like to do. So rather than do that this supports an alternate way of accomplishing the same thing.

You can specify an output code block by simply denoting a regular code block, but using two backticks (\`) instead of just one to delimit. Then the code you want to appear in the result is included inside. Code inside the backtick will be passed on literally to the output format, the processor doesn't check, or care about the syntax. This means that it'll work for any output format that it supports, and it doesn't tie the processor down to just html.

What if you want to support compiling the document to multiple formats? You can't include an output code block that's meant for LaTeX in html, so the processor will let you specify a specific output for the code block. Simply start the code block off with two percentage signs followed by the output format followed by another two percentage signs like so:

	``%%latex%% $\frac{1}{2}$``

The output format name is not case sensitive, and will be specified for each format. This output code block will only appear if the document is compiled to LaTeX, and will be ignored otherwise. This is much more flexible, and doesn't tie the processor to any specific language, and even a single document can support multiple output formats with sections of the output code inside it.

Of course this is an advanced feature and so most people are unlikely to use this, but advanced users can, and can expose this to other users by using text macros (explained below).

###Scope Regions###

This feature is really not much of a feature in and of itself. It's a way to specify a region of text as a certain scope, which allows you to treat it in special ways for text macros and output classes. You simply surround a section of text with \{ \} as shown

	{This text is all considered to be in the same "scope"}

More about how they are used is below in the next two features.

###Output Classes###

Most output formats support adding some sort of flexible node that can be styled/controlled externally. In HTML, you can use spans with classes, in LaTeX you can simply use commands. This feature makes it very easy to add custom elements with specialized styling (which is specified in the layout document that the processor uses, which can be the default, or a custom layout document you give it). All you have to do is put tildes around the class name, and then include the content in a scope region. For instance:

	~customClass~{This is an element with a custom class applied to it}

Will be transformed into something that supports a custom class, depending on the format (for formats that don't support custom classes or elements, then the text is just inserted as is). For instance as an html backend it will become:

	<span class="customClass">This is an element with a custom class applied to it</span>

and for LaTeX it will become:

	\customClass{This is an element with a custom class applied to it}

This is a quick and easy way to add custom elements to markdown, however it does require including the customized logic within the layout document, for a way to specify this within markdown we can use macros. 

###Commands/Text Macros###

A command or text macro is a way to extend markdown yourself. Oftentimes in documents you may want to refer to the name of something, but that name might change, or be unknown, and it's annoying to find and replace all instances of it (and it's possible to miss a few), so it'd be useful to support inserting a "variable". This is supported by delimiting a word with dollar signs like so:

	$productName$

And you can define it in the document, similar to the way links are defined: (on it's own line, the same word delimited with dollar signs, then a colon and a space, followed by the value)

	$productName$: Markdown Processor

Of course you don't have to just use it for names, you can use it for lots of things. It could be a convenient way to specify the current year or date for instance:

	It is currently the year $year$ and today is $date$

	$year$: 2013
	$date$: December 22, 2013

The processor even exposes specific variables, and these can be accessed by starting the name off with an underscore. The above can be written as simply:

	It is currently the year $_year$ and today is $_date$

And the processor will fill it in with the current year and date.

A very useful use for these macros is to extend markdown with commands supported in the outputs. You can support multiple outputs with a single command just by placing each one in a language output code section, and it'll only output with that format. For instance, you can extend markdown to create tables with the following definition:

	$tableBegin$: ``%%html%%<table>``
	$tableRowBegin$: ``%%html%%<tr><td>``
	$tableRowSeperator$: ``%%html%%</td><td>``
	$tableRowEnd$: ``%%html%%</td></tr>``
	$tableEnd$: ``%%html%%</table>``

It can be used like the following:

	$tableBegin$
	$tableRowBegin$ Items $tableRowSeperator$ Prices $tableRowEnd$
	$tableRowBegin$ Apples $tableRowSeperator$ \$2.00 $tableRowEnd$
	$tableRowBegin$ Oranges $tableRowSeperator$ \$1.00 $tableRowEnd$
	$tableRowBegin$ Bananas $tableRowSeperator$ \$3.00 $tableRowEnd$
	$tableRowBegin$ Grapefruit $tableRowSeperator$ \$0.50 $tableRowEnd$
	$tableEnd$

That will insert all the appropriate commands when the document is compiled for html, and you can easily specify alternate syntax for other formats just by including it on the same line like so:

	$tableBegin$: ``%%hmtl%%<table>`` ``%%latex%%\begin{tabular}``

This makes markdown completely extendable, and you can import "libraries" which consist of markdown documents containing macros like this.

Of course macros that just insert stuff is useful, but in order to be truly useful you need to be able to specify arguments. When you define a macro, you may optionally specify a number. This number comes after the last `$`, before the colon. These are the number of arguments, and they are assigned to the variables `$0$` all the way up to the number you specified minus one. For instance let's say you add a macro listItem which makes a list that looks like below:

+ **First Item** - description
+ **Second Item** - description

You can define the command like the following:

	$listItem$2: + **$0$** - $1$

You use it with the scopes command from above, like the following:

	$listItem${First Item}{description}
	$listItem${Second Item}{description}

Of course using it like this means that it's a little more work to type, but it means if you want to change the way list items work, you only need to change the command.

The following consists of more advanced stuff, so if you don't plan on using advanced features, you can skip this section.

If you want to specify a newline within these macros, you need to use `\n` since you can't have macros extend multiple lines.

Before we dive into the examples below, a quick bit of syntatic sugar. If you want to specify a command that simply uses the output code blocks for various languages, you can separate them into macros, each with a `%%output%%` after the colon but before the space. For instance the following:

	$newline$: ``%%html%% <br />````%%latex%% \\ ````%%word%% \n ``

can be transformed into the following 3 lines:

	$newline$:%%html%%   <br />
	$newline$:%%latex%%  \\
	$newline$:%%word%%   \n

which will be transformed into the same thing as above. This makes it easier to specify commands for multiple languages.

Normally all text inside the output code blocks are completely ignored, and simply passed on to the output processor, but for convenience, within these macros, the dollar signs are used as they normally would be, and allow you to call macros as you could normally, but without having to exit the block, and enter it again. For instance without this syntatic sugar you'd have to do the following:

	$paragraph$1: ``%%html%%<p>``$0$``%%html%%</p>``

but with the syntatic sugar, you can simply do:

	$paragraph$`:%%html%%   <p>$0$</p>

Which greatly reduces the noise, and makes it much easier to write and read. Note that the macro syntax (`$macro$`) is the only thing that works within it. So if you want to use the rest of markdown's syntax within the macro, you won't be able to use this alternate syntax.

For macros that take an unspecified number of arguments, you can use the special values n and 0. With these you actually specify the command twice, once for the first n elements, and once for the last one. With n, `$0$` is the actual value, and `$1$` tells it where to put the rest, so it actually calls your command recursively. Then the command with 0 is called for when there's only one argument left, and it's bound to variable `$0$`. For instance:

	$tableRowElements$n:%%html%%   <td>$0$</td> $1$
	$tableRowElements$0:%%html%%   <td>$0$</td>

Will take any number of arguments and surround each with `<td></td>` to make. You can also exclude the last one, and it'll just call the nth one again for the last one, but this time `$1$` will be the empty string.

Alternatively you can pass along the entire list to another macro, after optionally adding your own processing. We can make the whole table row by adding a new macro:

	$tableRow$n:%%html%%   <tr>$tableRowElements${$n$}</tr>

This will surround the entire list with `<tr></tr>` and then create the list using the earlier macro. We now can create an entire table row with one macro, and by adding another 2 macros, we can create the whole table. The following 2 macros are very similar to above, but their arguments are actually another list of arguments, rather than a single argument. So inside the macro, `$0$` refers to the nth argument, which is actually a list of arguments, so you can use it to call a macro that takes n arguments (similar to how you can pass along the whole list to another macro, like in `tableRow` macro above)

	$tableRows$n:%%html%%   <tr>$tableRow${$0$}</tr> \n $1$
	$table$n:%%html%%   <table>$tableRows${$n$}</table>

And now with these 5 macros (or rather 4, since the `$tableRowElements$0` line can be removed) we have added table support to markdown, and can use it in the file with the following:

	$table$
		{{Items}{Prices}}
		{{Apples}{\$2.00}}
		{{Oranges}{\$1.00}}
		{{Bananas}{\$3.00}}
		{{Grapefruit}{\$0.50}}

Which is much easier to use than the earlier syntax.

This macro feature is slightly complicated to write macros for, more complicated than regular markdown, but macros aren't something the typical user will write. The most is they may use the macros, which is fairly simple to understand and use. 

The benefit is that markdown becomes very customizable, and can be extended to support new features that don't exist in markdown, but may exist in the output formats. This simple addition to the language makes it possible for Markdown to be more flexible than html, as it can support everything html supports plus a lot extra. To see that you can do everything you can do in markdown, look in markdownAsHTML.md, which shows some macros that lets you do everything you can do in HTML (plus some extra).


> Note: This extra features are not yet supported, and might be a while before being supported, so if you have suggestions about how to better implement them, please feel free to open a ticket to discuss it, these are very flexible since the code has not yet been written.

What happens if you call an non-list macro with a list? For instance what happens if you call our `listItem` macro with 4 parameters?

	$listItem${first item}{description}{second item}{description}

Well it simply calls the macro twice. Specifically the following steps are taken:

1. Put all arguments into a queue (a list)
+ Find the macro, and how many arguments it takes.
+ Take as many arguments from the front of the queue as the macro will take
	+ If there aren't enough, just replace any missing arguments with the empty string
+ Call the macro with the arguments
+ If there are any arguments left over go to step 3.

This is more than just convenience for not calling the same thing multiple times for the same macro, it also lets you call a function that takes a list of arguments with a specific number, or one that takes a specific number with a list.
 

Contribution Help
--------------

You can contribute by extending the parser to support more of the core syntax, or by adding a new backend. We also need some more test cases. You can also contribute simply by using it and reporting bugs.