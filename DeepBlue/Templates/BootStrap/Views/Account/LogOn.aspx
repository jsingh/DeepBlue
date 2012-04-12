<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Account.LogOnModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<!DOCTYPE html>
<html lang="en">
<head>
	<meta charset="utf-8" />
	<title>LogOn</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />
	<%= Html.JavascriptInclueTag("jquery-1.7.1.min.js")%>
	<%= Html.JavascriptInclueTag("jquery.tmpl.min.js")%>
	<script src="<%:Url.Content("~/Templates/BootStrap/Assets/javascripts/LogOn.js")%>"	type="text/javascript"></script>
	<link href="<%:Url.Content("~/Templates/BootStrap/Assets/stylesheets/bootstrap.min.css")%>"	rel="stylesheet" type="text/css" />
	<link href="<%:Url.Content("~/Templates/BootStrap/Assets/stylesheets/site.css")%>" rel="stylesheet" type="text/css" />
</head>
<body>
	<div class="navbar navbar-fixed-top">
		<div class="navbar-inner">
			<div class="container-fluid">
				<a class="brand" href="#">
					<%=EntityHelper.EntityName%></a>
			</div>
		</div>
	</div>
	<div class="container-fluid">
		<div class="row-fluid" id="AlertRow">
		</div>
		<div class="row-fluid">
			<div class="span4">
				&nbsp;</div>
			<div class="span4">
				<% using (Html.BeginForm("LogOn", "Account", FormMethod.Post, new { @class = "form-horizontal", @onsubmit = "return logon.submit(this); " })) { %>
				<fieldset>
					<legend>Login</legend>
					<div class="control-group">
						<%: Html.LabelFor(m => m.UserName, new { @class = "control-label" })%>
						<div class="controls">
							<%: Html.TextBoxFor(m => m.UserName)%>
						</div>
					</div>
					<div class="control-group">
						<%: Html.LabelFor(m => m.Password, new { @class = "control-label" })%>
						<div class="controls">
							<%: Html.PasswordFor(m=> m.Password)%>
						</div>
					</div>
					<div class="control-group">
						<%: Html.LabelFor(m => m.EntityCode, new { @class = "control-label" })%>
						<div class="controls">
							<%: Html.TextBoxFor(m   => m.EntityCode)%>
						</div>
					</div>
					<div class="control-group">
						<div class="controls">
							<label class="checkbox">
								<%: Html.CheckBoxFor(m => m.RememberMe)%>
								RememberMe
							</label>
						</div>
					</div>
					<div class="form-actions">
						<button class="btn btn-primary" type="submit">
							Login</button>
					</div>
				</fieldset>
				<%: Html.ValidationMessageFor(m => m.UserName)%>
				<%: Html.ValidationMessageFor(m => m.Password)%>
				<%: Html.ValidationMessageFor(m => m.EntityCode)%>
				<%: Html.ValidationMessageFor(m => m.Errors)%>
				<%: Html.Hidden("ReturnUrl", ViewData["ReturnUrl"])%>
				<%}%>
			</div>
			<div class="span4">
				&nbsp;</div>
		</div>
		<%Html.RenderPartial("Footer");%>
	</div>
	<%using (Html.jQueryTemplateScript("alertTemplate")) {%>
	<div class="span4">
		&nbsp;</div>
	<div class="span4">
		{{if iswarning==true}}
		<div class="alert">
			<a class="close" data-dismiss="alert">×</a>{{html message}}
		</div>
		{{else}}
		<div class="alert alert-error">
			<a data-dismiss="alert" class="close">×</a>{{html message}}
		</div>
		{{/if}}
	</div>
	<div class="span4">
		&nbsp;</div>
	<%}%>
	<script type="text/javascript">
		$(document).ready(function () { logon.init(); }); </script>
</body>
<script src="<%:Url.Content("~/Templates/BootStrap/Assets/javascripts/bootstrap/bootstrap-alert.js")%>"
	type="text/javascript"></script>
</html>
