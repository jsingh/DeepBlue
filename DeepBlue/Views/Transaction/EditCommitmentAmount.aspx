<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/DeepBluePopup.Master" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Transaction.EditCommitmentAmountModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
	<%= Html.JavascriptInclueTag("EditTransaction.js")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<%Html.EnableClientValidation(); %>
	<% using (Ajax.BeginForm("UpdateCommitmentAmount", new AjaxOptions { OnBegin = "editTransaction.onBegin", OnSuccess = "editTransaction.closeDialog" })) {%>
	<%: Html.ValidationSummary(true) %>
	<%: Html.HiddenFor(model => model.InvestorFundId) %>
	<br />
	<div class="editor-field small-text auto-width">
		<%: Html.LabelFor(model => model.CommitmentAmount) %>&nbsp;<%: Html.TextBoxFor(model => model.CommitmentAmount, String.Format("{0:F}", Model.CommitmentAmount)) %>
		<%: Html.ValidationMessageFor(model => model.CommitmentAmount) %>
	</div>
	<div class="editor-button" style="width: 225px; padding-top: 10px;">
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.Span("",new { id = "UpdateLoading" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageButton("Update.png", new { style = "width: 73px; height: 23px;", onclick = "return editTransaction.onSubmit();" })%>
		</div>
		<div style="float: left; padding: 0 0 10px 5px;">
			<%: Html.ImageLink("Close.png", new { style = "width: 73px; height: 23px;cursor:pointer;", onclick = "editTransaction.closeDialog();" })%>
		</div>
	</div>
	<% } %>
</asp:Content>
