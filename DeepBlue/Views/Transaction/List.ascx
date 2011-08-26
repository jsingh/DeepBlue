<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<DeepBlue.Models.Entity.InvestorFund>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% foreach (var item in Model) { %>
<div class="fund-detail">
	<div class="tab">
		<div class="tab-left">
		</div>
		<div class="tab-center">
			<%: Html.Label("Investor In") %>&nbsp;<%: Html.Span(item.Fund.FundName)%></div>
		<div class="tab-right">
		</div>
		<div class="tab-cnt">
			<div class="editor-row">
				<div class="editor-label">
					<%: Html.Label("Committed Amount") %>
				</div>
				<div class="display-field">
					<%: Html.Span(FormatHelper.CurrencyFormat(item.TotalCommitment))%>
				</div>
			</div>
			<div class="editor-row">
				<div class="editor-label">
					<%: Html.Label("Unfunded Amount")%>
				</div>
				<div class="display-field">
					<%: Html.Span(FormatHelper.CurrencyFormat(item.UnfundedAmount))%>
				</div>
			</div>
			<div class="editor-link">
				<%: Html.Anchor("Transaction", "#", new { onclick = "javascript:transactionController.editTransaction("+ item.InvestorFundID.ToString() +");" })%>&nbsp;-&nbsp;
				<%: Html.Anchor("Edit Committed Amount", "#", new { onclick = "javascript:transactionController.editCommitmentAmount(" + item.InvestorFundID.ToString() + ");" })%>
			</div>
		</div>
	</div>
</div>
<%}%>
