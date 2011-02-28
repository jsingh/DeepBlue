<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Transaction.EditCommitmentAmountModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%Html.EnableClientValidation(); %>
<% using (Ajax.BeginForm("UpdateCommitmentAmount", new AjaxOptions { UpdateTargetId="UpdateTargetId", OnBegin = "editTransaction.onBegin", OnSuccess = "editTransaction.closeEditCommitAmtDialog" })) {%>
<%: Html.ValidationSummary(true) %>
<%: Html.HiddenFor(model => model.InvestorFundId) %>
<br />
<div class="editor-field small-text auto-width">
	<%: Html.LabelFor(model => model.CommitmentAmount) %>&nbsp;<%: Html.TextBox("CommitmentAmount", (Model.CommitmentAmount <= 0 ? "" : String.Format("{0:F}", Model.CommitmentAmount)), new { @id = "CommitmentAmount" }) %>
	<%: Html.ValidationMessageFor(model => model.CommitmentAmount) %>
</div>
<div class="editor-button" style="width: 225px; padding-top: 10px;">
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Span("",new { id = "UpdateEditCmtLoading" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.ImageButton("Update.png", new { style = "width: 73px; height: 23px;", onclick = "return editTransaction.onSubmit();" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Image("Close.png", new { style = "width: 73px; height: 23px;cursor:pointer;", onclick = "editTransaction.closeEditCommitAmtDialog();" })%>
	</div>
	<div id="UpdateTargetId">
	</div>
</div>
<% } %>
