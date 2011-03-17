<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Admin.EditFundClosingModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditFundClosing
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("FundClosing.js")%>
	<%= Html.JavascriptInclueTag("jquery-ui-1.8.10.custom.min.js")%>
	<%= Html.StylesheetLinkTag("jquery-ui-1.8.10.custom.css")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="fund-closing-edit">
		<%Html.EnableClientValidation(); %>
		<% using (Ajax.BeginForm("UpdateFundClosing", null,
	 new AjaxOptions {
		 UpdateTargetId = "UpdateTargetId", HttpMethod = "Post",
		 OnBegin = "fundClosing.onCreateFundClosingBegin", OnSuccess = "fundClosing.onCreateFundClosingSuccess"
	 }, new { @id = "AddNewFundClosing" })) {%>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.Name) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBoxFor(model => model.Name) %>
			<%: Html.ValidationMessageFor(model => model.Name) %>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.FundID) %>
		</div>
		<div class="editor-field">
			<%: Html.DropDownListFor(model => model.FundID,Model.FundNames, new { @onchange = "javascript:fundClosing.changeFund(this);" })%>
			<%: Html.ValidationMessageFor(model => model.FundID) %>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.FundClosingDate) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBox("FundClosingDate",
								 (Model.FundClosingDate.HasValue ? (Model.FundClosingDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy") : ""), new { @id = "FundClosingDate" })%>
			<%: Html.ValidationMessageFor(model => model.FundClosingDate) %>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.IsFirstClosing) %>
		</div>
		<div class="editor-field">
			<%: Html.CheckBoxFor(model => model.IsFirstClosing, new { @style = "width:auto" })%>
		</div>
		<div class="status">
			<%: Html.Span("", new { id = "UpdateLoading" })%></div>
		<div class="editor-button" style="width: 200px">
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.ImageButton("Save.png", new { style = "width: 73px; height: 23px;", onclick = "return fundClosing.onSubmit('AddNewFundClosing');" })%>
			</div>
			<div style="float: left; padding: 0 0 10px 5px;">
				<%: Html.Image("Close.png", new { style = "width: 73px; height: 23px;cursor:pointer;", onclick = "javascript:parent.fundClosing.closeDialog(false);" })%>
			</div>
		</div>
	</div>
	<%: Html.HiddenFor(model => model.FundClosingID) %>
	<% } %>
	<div id="UpdateTargetId" style="display: none">
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomContent" runat="server">
	<%= Html.jQueryDatePicker("FundClosingDate", new DatePickerOptions { OnBeforeShow = "fundClosing.showDate", OnClose = "fundClosing.closeDate" })%>

	<script type="text/javascript">
		fundClosing.init();
	</script>

</asp:Content>
