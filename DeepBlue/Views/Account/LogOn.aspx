<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Account.LogOnModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>LogOn</title>
	<%= Html.JavascriptInclueTag("jquery-1.4.1.min.js")%>
	<%= Html.StylesheetLinkTag("site.css")%>
	<%= Html.StylesheetLinkTag("logon.css")%>
</head>
<body>
	<div id="header">
		<div class="topheader" id="topheader">
			<div class="cname">
				WILLOWRIDGE</div>
		</div>
	</div>
	<div id="content">
		<div id="logon">
			<div id="logonheader">
				<div style="padding-left: 5px">
					Login</div>
			</div>
			<div id="logonmain">
				<% using (Html.BeginForm()) { %>
				<div class="editor-label">
					<%: Html.LabelFor(m => m.UserName) %>
				</div>
				<div class="editor-field">
					<%: Html.TextBoxFor(m => m.UserName) %>
					<%: Html.ValidationMessageFor(m => m.UserName) %>
				</div>
				<div class="editor-label">
					<%: Html.LabelFor(m => m.Password) %>
				</div>
				<div class="editor-field">
					<%: Html.PasswordFor(m => m.Password) %>
					<%: Html.ValidationMessageFor(m => m.Password) %>
				</div>
				<div class="editor-field clear" style="float: right">
					<%: Html.CheckBoxFor(m => m.RememberMe) %>
					<%: Html.LabelFor(m => m.RememberMe) %>
				</div>
				<div class="editor-field clear" style="float: right">
					<%: Html.ImageButton("Login.png")%>
				</div>
				<%: Html.ValidationMessageFor(m => m.Errors)%>
				<% } %>
			</div>
		</div>
	</div>
	<%Html.RenderPartial("Footer");%>
	<script type="text/javascript">
		$(document).ready(function () {
			var err="";
			$(".field-validation-error").each(function () { err+=this.innerHTML+"\n"; });
			if($.trim(err)!="") {
				alert(err);
			}
		});
	</script>
</body>
</html>
