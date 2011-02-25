<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<DeepBlue.Models.Entity.InvestorFund>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% foreach (var item in Model) { %>
<div class="fund-detail">
	<div class="editor-label">
		<%: Html.Label("Investor In:") %>
	</div>
	<div class="display-field">
		<%: Html.Span(item.Fund.FundName)%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Committed Amount:") %>
	</div>
	<div class="display-field">
		<%: Html.Span(item.TotalCommitment.ToString())%>
	</div>
	<div class="editor-label">
		<%: Html.Label("Unfunded Amount:")%>
	</div>
	<div class="display-field">
		<%: Html.Span(item.UnfundedAmount.ToString())%>
	</div>
	<div class="editor-link">
		<% foreach (var transaction in item.InvestorFundTransactions) {%>
		<%: Html.Anchor("Transaction", "#", new { onclick = "javascript:transactionController.editTransaction("+ transaction.InvestorFundTransactionID.ToString() +");" })%>
		<%}%>
	</div>
	<div class="editor-link">
		<%: Html.Anchor("Edit Committed Amount", "#", new { onclick = "javascript:transactionController.addTransaction(" + item.InvestorFundID.ToString() + ");" })%>
	</div>
</div>
<%}%>
