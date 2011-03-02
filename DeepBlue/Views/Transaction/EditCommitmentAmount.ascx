<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Transaction.EditCommitmentAmountModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%Html.EnableClientValidation(); %>
<% using (Ajax.BeginForm("UpdateCommitmentAmount", new AjaxOptions {
	   UpdateTargetId = "UpdateTargetId", Confirm = "Are you sure you want to update this commitment amount?", OnBegin = "editTransaction.onBegin", OnSuccess = "editTransaction.closeEditCommitAmtDialog"
   })) {%>
<%: Html.ValidationSummary(true) %>
<%: Html.HiddenFor(model => model.InvestorFundId) %>
<%: Html.HiddenFor(model => model.UnfundedAmount) %>
<br />
<div class="editor-row">
	<div class="editor-label" style="width:auto;">
		<%: Html.LabelFor(model => model.CommitmentAmount) %>
	</div>
	<div class="editor-field" style="width:auto;">
		<%: Html.TextBox("CommitmentAmount", (Model.CommitmentAmount <= 0 ? "" : String.Format("{0:F}", Model.CommitmentAmount)), new { @id = "CommitmentAmount", @style="width:100px" }) %>
		<br />
		<%: Html.ValidationMessageFor(model => model.CommitmentAmount) %>
	</div>
</div>
<div class="editor-button" style="width: 225px; padding-top: 10px;">
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Span("",new { id = "UpdateEditCmtLoading" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.ImageButton("Update.png", new { style = "width: 73px; height: 23px;", onclick = "return editTransaction.onEditCommitAmgSubmit();" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Image("Close.png", new { style = "width: 73px; height: 23px;cursor:pointer;", onclick = "editTransaction.closeEditCommitAmtDialog();" })%>
	</div>
</div>
<div id="UpdateTargetId"></div>
<% } %>
