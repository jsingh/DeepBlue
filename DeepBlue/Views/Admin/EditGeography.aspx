﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master"
	Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditGeographyModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditGeography
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("Geography.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateGeography", null, new AjaxOptions { UpdateTargetId = "UpdateTargetId", HttpMethod = "Post", OnBegin = "geography.onGeographyBegin", OnSuccess = "geography.onGeographySuccess" }, new { @id = "AddNewGeography" })) {%>
	<div class="editor-label auto-width">
		<%: Html.LabelFor(model => model.Geography) %>
	</div>
	<div class="editor-field">
		<%: Html.TextBoxFor(model => model.Geography) %>
		<%: Html.ValidationMessageFor(model => model.Geography) %>
	</div>
	<div class="editor-label" style="width: 63px">
		<%: Html.LabelFor(model => model.Enabled)%>
	</div>
	<div class="editor-field">
		<%: Html.CheckBoxFor(model => model.Enabled, new { @style = "width:auto" })%>
	</div>
	<div class="status">
		<%: Html.Span("", new { id = "UpdateLoading" })%></div>
	<div class="editor-button" style="width: 200px">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 26px;", onclick = "return geography.onSubmit('AddNewGeography');" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Image("Close.png", new { style = "width: 73px; height: 26px;cursor:pointer;", onclick = "javascript:parent.geography.closeDialog(false);" })%>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.GeographyId) %>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<script type="text/javascript">
		geography.init();
	</script>
</asp:Content>
