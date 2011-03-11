﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Document.UploadModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	New
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.StylesheetLinkTag("document.css")%>
	<%=Html.JavascriptInclueTag("DocumentUpload.js")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="document-upload">
		<% Html.EnableClientValidation(); %>
		<% using (Html.BeginForm("Create", "Document", FormMethod.Post, new { @id = "AddNewDocument", @enctype = "multipart/form-data" })) {%>
		<%: Html.HiddenFor(model => model.InvestorId)%>
		<%: Html.HiddenFor(model => model.FundId)%>
		<div class="editor-label" style="width: 200px">
			<b>Document Upload</b>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.DocumentTypeId) %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownListFor(model => model.DocumentTypeId,Model.DocumentTypes) %>
			<%: Html.ValidationMessageFor(model => model.DocumentTypeId) %>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.DocumentDate) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("DocumentDate","",new { @id = "DocumentDate" }) %>
			<%: Html.ValidationMessageFor(model => model.DocumentDate) %>
		</div>
		<div class="editor-label">
			<%: Html.DropDownListFor(model => model.DocumentStatus,Model.DocumentStatusTypes, new { @onchange = "javascript:documentUpload.changeType(this);" })%>
		</div>
		<div class="editor-field">
			<div id="InvestorRow">
				<%: Html.TextBoxFor(model => model.InvestorName, new { @onblur="javascript:documentUpload.InvestorBlur(this);" }) %>
				<%: Html.ValidationMessageFor(model => model.InvestorId) %>
			</div>
			<div id="FundRow" style="display: none">
				<%: Html.TextBoxFor(model => model.FundName, new { @onblur = "javascript:documentUpload.FundBlur(this);" })%>
				<%: Html.ValidationMessageFor(model => model.FundId) %>
			</div>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.FileName) %>
		</div>
		<div class="editor-field">
			<%: Html.File("FileName", new { @id = "FileName" })%>
			<%: Html.ValidationMessageFor(model => model.FileName)%>
		</div>
		<div class="editor-label" style="height: 15px">
			<%: Html.ValidationMessageFor(model => model.ModelErrorMessage)%>
			<%: Html.Span("",new { id = "UpdateLoading" })%>
		</div>
		<div class="editor-button" style="width: 165px">
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 23px;", onclick = "return documentUpload.onSubmit('AddNewDocument');" })%>
			</div>
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.Image("Close.png", new { style = "width: 73px; height: 23px;cursor:pointer;", onclick = "javascript:parent.documentUpload.closeDialog(false);" })%>
			</div>
		</div>
	</div>
	<% } %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryDatePicker("DocumentDate")%>
	<%= Html.jQueryAutoComplete("InvestorName", new AutoCompleteOptions {
																	  Source = "/Investor/FindInvestors", MinLength = 1,
																	  OnSelect = "function(event, ui) { documentUpload.selectInvestor(ui.item.id);}"})%>
	<%= Html.jQueryAutoComplete("FundName", new AutoCompleteOptions {
																	  Source = "/Fund/FindFunds", MinLength = 1,
																	  OnSelect = "function(event, ui) { documentUpload.selectFund(ui.item.id);}"})%>

	<script type="text/javascript">
		$(document).ready(function () {
			var frm=document.getElementById("AddNewDocument");
			var DocumentStatus = document.getElementById("DocumentStatus");
			documentUpload.changeType(DocumentStatus);
			documentUpload.showErrorMessage(frm);
		});
	</script>

</asp:Content>
