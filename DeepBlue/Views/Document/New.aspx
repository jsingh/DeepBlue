<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBlue.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Document.CreateModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Document
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="server">
	<%=Html.StylesheetLinkTag("document.css")%>
	<%=Html.JavascriptInclueTag("jquery.fileuploader.js")%>
	<%=Html.JavascriptInclueTag("DocumentUpload.js")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="NavigationContent" runat="server">
	<div class="navigation">
		<div class="heading">
			<div class="leftcol">
				<div class="title">
					DOCUMENT MANAGEMENT</div>
				<div class="arrow">
				</div>
				<div class="pname">
					DOCUMENT UPLOAD</div>
			</div>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="doc-upload">
		<% using (Html.Form(new { @id = "AddNewDocument", @onsubmit = "return documentUpload.save(this);", @enctype = "multipart/form-data" })) {%>
		<%: Html.HiddenFor(model => model.InvestorId)%>
		<%: Html.HiddenFor(model => model.FundId)%>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.DocumentTypeId) %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownListFor(model => model.DocumentTypeId,Model.DocumentTypes) %>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.DocumentDate) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("DocumentDate","",new { @id = "DocumentDate" }) %>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.DocumentStatus)%>
		</div>
		<div class="editor-field">
			<div id="InvestorRow" style="float: left;">
				<%: Html.TextBoxFor(model => model.InvestorName, new { @onblur = "javascript:documentUpload.InvestorBlur(this);", @style = "width:196px" })%>
			</div>
			<div id="FundRow" style="display: none; float: left;">
				<%: Html.TextBoxFor(model => model.FundName, new { @onblur = "javascript:documentUpload.FundBlur(this);", @style = "width:196px" })%>
			</div>
			<div style="float: left; margin-left: 2px;">
				<%: Html.DropDownListFor(model => model.DocumentStatus,Model.DocumentStatusTypes, new { @style="width:80px", @onchange = "javascript:documentUpload.changeType(this);" })%>
			</div>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.FilePath)%>
		</div>
		<div class="editor-field">
			<div id="FileRow" style="float: left;width:221px;">
				<%: Html.File("File", new { @id = "File"  })%>
			</div>
			<div id="LinkRow" style="display: none; float: left;">
				<%: Html.TextBoxFor(model => model.FilePath, new { @style = "width:213px" })%>
			</div>
			<div style="float: left; margin-left: 2px;">
				<%: Html.DropDownListFor(model => model.UploadType, Model.UploadTypes, new { @style = "width:80px", @onchange = "javascript:documentUpload.changeUploadType(this);" })%>
			</div>
		</div>
		<div class="editor-button" style="width: auto; padding: 10px 0 0;">
			<div style="float: left; padding: 0 0 10px 50px;">
				<%: Html.ImageButton("Save90.png", new { @class = "default-button" })%>
			</div>
			<div style="float: left; padding: 0;">
				<%: Html.Span("", new { @id = "SpnDocLoading" })%>
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
		$("#cntbox").css("top","47px");
			var DocumentStatus=document.getElementById("DocumentStatus");
			documentUpload.changeType(DocumentStatus);
			var UploadType=document.getElementById("UploadType");
			documentUpload.changeUploadType(UploadType);
			documentUpload.init();
			
		});
	</script>
</asp:Content>
