Markdown As HTML
===

> Note: Before reading this document, make sure you've read the readme, specifically the section on new additional features. This document assumes you've read and understand that first.

Markdown is traditionally seen as a simple formatting language, which outputs to HTML, and lets you maybe specify html within the code, but is at best as flexible as HTML, by writing html code. HTML is not very flexible by itself though, as seen by countless frameworks like [AngluarJS][angular] and [EmberJS][ember]. HTML is a very static, inflexible format, and doesn't allow for any customization. When combined with CSS and javascript, it can be faked to be that powerful, but HTML itself isn't a particularly readable or flexible format.

Now these are bold statements to make, to say that this relatively simple format is more flexible than HTML, and proof is needed to back those statements up. This document will outline the proof, and the proof will consist of 2 parts. The first will be a series of macros that will let you specify everything in HTML (this is not something new to markdown, most processors let you write inline HTML, however this one does not, so proof is needed to show you can output all html within it). The second part will consist of showing some feature that HTML can't do, which makes it more flexible and customizable.

Be aware while reading this proof, this only applies to this flavour of Markdown, and not markdown in general.

Make HTML within Markdown
---

A very obvious and boring proof would be to use output code blocks, and just do

	``%%html%% <html>
				<head>
				</head>
				<body>
				</body>
				</html>``

and so on. This does in fact show you can do all html within the document, but it doesn't really demonstrate it in a very markdown way. Rather let's define a few macros:

	$html$3:%%html%%         <$0$ $attributes${$1$}>$2$</$0$>
	$attribute$2:%%html%%	 $0$='$1$'
	$attributes$n:%%html%%   $attribute${$0$} $1$

With these macros, you can write any html node by doing the following:

	$html${p}{{style}{color:green;}}{This is a green paragraph}

They can of course be nested

	$html${body}{}{
		$html${p}{}{This is a paragraph in a body}
	}

So with these macros we can write all the HTML code we want within markdown. As a bonus we can do something quite neat. We can make these macros work with LaTeX as well.

	$html$3:%%latex%%	\\$0$\{$attributes${$1$}\}{$2$}
	$attribute$2:%%html%%	 \attribute_$0$\{$1$\}
	$attributes$n:%%html%%   $attribute${$0$} $1$

This will expand the above green paragraph into:

	\p{\attribute_style{color:green;}}{This is a green paragraph}

And as long as we include a LaTeX layout file that defines all the HTML elements and attributes, we can get LaTeX output from this HTML based macro.

Customizable, flexible things Markdown can do
---

There are many things Markdown can do that make it much more flexible and customizable than HTML. Here are a few of them:

###Conditional Compilation###

This is more of a byproduct than on purpose but because of the way output code blocks work, it's possible to do conditional compilation with Markdown. A very simple example is:

	People who use $format$ are awesome.

	$format$:%%html%% HTML
	$format$:%%latex%% \LaTeX{}
	$format$:%%word%% Word documents
	
Whatever format it is compiled to, it will tell those people that they are awesome. When compiled with HTML it will say those people are awesome, and when compiled with LaTeX, it will say those people are awesome (it even formats the word LaTeX with the LaTeX command). It does the same with word, and could be extended for other formats.

This is kinda neat, but the really cool part comes from a little abuse of the output format matching. If we tell the processor to define an additional output format, say `debug`, then we can check for that in the code. We can use this to conditionally include sections if debug mode is on. Here's an example.

	Title
	===
	$note${Hey Steve, if you're reading this, can you take a look at the following section? It might need a little reworking}
	Lorem ipsum lorem ispum lorem ipsum
	
	$note$1:%%debug%% $0$

The note only shows up if you tell the processor to define the `debug` format as an additional format. So this note only shows up when the document is compiled in debug mode, so Steve will see it, but the end-user won't.

Note that we could also have defined it like this:

	$note$1: {}

And that make the `note` macro never output anything, no matter what arguments it was given. This can be used to define a comment macro (In fact the system defines a macro `$_comment$` just for this purpose)

Conditional compilation is definitely something that HTML can't do, and a very useful feature to have. This already makes it more powerful than HTML, but let's continue with some more features

###Import file###

A very common problem with HTML is that you can't import another file into it. This means it requires an external processor to separate content into several different files. It's extremely common to have pretty much the same `head` section in every page on a site, as well as a nav bar, so you have the choice of either duplicating the content across every page (which is very bad), or using a different format that compiles to HTML (like PHP or ASP.NET). So that a pre-processor wouldn't need to be required just for a simple feature, there is a system macro in Markdown that will include a file for you. (`$_import$` and `$_importRaw$`).


###Conditionals###

###Extended System Macros###

You can extend the system macros in a very easy way. All you have to do is tell the processor about a program or script that accepts the name of a macro as well as optional arguments. The script/program returns the resulting text in stdout or `fail` in stderr if it doesn't understand the macro. Here's an example:

The markdown file contains the following:

	I think $employeeName${117} deserves a raise

	$employeeName$1: $_sqlQuery${select name from dbo.employee e where e.employeeID = $0$}

The Markdown processor will come across the `_sqlQuery` command, see that it's not defined internally, and run the following command:

	externalScript sqlQuery "select name from dbo.employee e where e.employeeID = 117"

now the script has a chance to execute this query and return the result. This makes it VERY flexible, although this could increase the amount of time it takes to compile the document. 

[angular]: http://angularjs.org/  
[ember]: http://emberjs.com/