<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<!DOCTYPE html>
<html>
<head>
	<%=Html.JavascriptInclueTag("jquery-1.4.1.min.js")%>
	<%=Html.JavascriptInclueTag("jquery.filedrop.js")%>
	<%=Html.JavascriptInclueTag("FileUploadScript.js")%>
	<!--[if lt IE 9]>
		<%=Html.JavascriptInclueTag("html5.js")%>
     <![endif]-->
	<%=Html.StylesheetLinkTag("styles.css")%>
</head>
<body>
	<header>
			<h1>HTML5 File Upload with jQuery and PHP</h1>
		</header>
	<div id="dropbox">
		<span class="message">Drop images here to upload.
			<br />
			<i>(they will only be visible to you)</i></span>
	</div>
	<footer>
	        <h2>HTML5 File Upload with jQuery and PHP</h2>
            <a class="tzine" href="http://tutorialzine.com/2011/09/html5-file-upload-jquery-php/">Read &amp; Download on</a>
        </footer>
</body>
</html>
