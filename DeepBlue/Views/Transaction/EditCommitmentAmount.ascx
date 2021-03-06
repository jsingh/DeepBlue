﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Transaction.EditCommitmentAmountModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<%Html.EnableClientValidation(); %>
<% using(Html.Form(new { @onsubmit = "return editTransaction.editCA(this);" })){ %>
<%: Html.HiddenFor(model => model.InvestorFundId) %>
<%: Html.HiddenFor(model => model.UnfundedAmount) %>
<br />
<div id="EditCommitAmtLoading">
</div>
<div class="editor-row">
	<div class="editor-label" style="width: auto;">
		<%: Html.LabelFor(model => model.CommitmentAmount) %>
	</div>
	<div class="editor-field" style="width: auto;">
		<%: Html.TextBox("CommitmentAmount", (Model.CommitmentAmount <= 0 ? "" : String.Format("{0:F}", Model.CommitmentAmount)), new { @id = "CommitmentAmount" }) %>
		<br />
		<%: Html.ValidationMessageFor(model => model.CommitmentAmount) %>
	</div>
</div>
<div class="editor-button" style="width: 225px; padding-top: 10px;">
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Span("",new { id = "UpdateEditCmtLoading" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.ImageButton("Update_active.png", new { @class="default-button" })%>
	</div>
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Image("Close_active.png", new { @class="default-button", onclick = "editTransaction.closeEditCommitAmtDialog();" })%>
	</div>
</div>
<% } %>
