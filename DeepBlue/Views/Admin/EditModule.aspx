﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditModule>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditModule
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("Module.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateModule", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "module.onModuleBegin", OnSuccess = "module.onModuleSuccess" }, new { @id = "AddNewModule" })) {%>
	<div class="editor-label auto-width">
		<%: Html.LabelFor(model => model.ModuleName) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.ModuleName) %>
		<%: Html.ValidationMessageFor(model => model.ModuleName) %>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { @class="default-button", onclick = "return module.onSubmit('AddNewModule');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { @class="default-button", onclick = "javascript:parent.module.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.ModuleID) %>
	<% } %>
	<div id="UpdateTargetId">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">

	<script type="text/javascript">
		module.init();
	</script>

</asp:Content>