﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditShareClassTypeModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditShareClassType
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("ShareClassType.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateShareClassType", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "shareClassType.onShareClassTypeBegin", OnSuccess = "shareClassType.onShareClassTypeSuccess" }, new { @id = "AddNewShareClassType" })) {%>
	<div class="editor-label auto-width">
		<%: Html.LabelFor(model => model.ShareClass) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.ShareClass) %>
		<%: Html.ValidationMessageFor(model => model.ShareClass) %>
	</div>
	<div class="editor-label" style="width: 70px">
		<%: Html.LabelFor(model => model.Enabled)%>
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.Enabled, new { @style = "width:auto" })%>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { @class="default-button", onclick = "return shareClassType.onSubmit('AddNewShareClassType');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { @class="default-button", onclick = "javascript:parent.shareClassType.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.ShareClassTypeId) %>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		shareClassType.init();
	</script>
</asp:Content>