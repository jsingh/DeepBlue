<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="editor-label">
	<%: Html.Label("Investor Name")%>
</div>
<div class="editor-field">
	<%: Html.Span("${InvestorName}", new { id = "InvestorName" })%>
</div>
<div class="editor-label">
	<%: Html.Label("Display Name")%>
</div>
<div class="editor-field">
	<%: Html.Span("${DisplayName}", new { id = "Spn_DisplayName" })%>
</div>
<div class="editor-label">
	<%: Html.Label("Notes")%>
</div>
<div class="editor-field" style="width:70%">
	<%: Html.Span("${formatEditor(Notes)}", new { @class="notes", id = "Spn_Notes" })%>
</div>
<div id="funddetails" class="fund-details">
	{{if FundInformations.length>0}}
	<div>
	<% Html.RenderPartial("TBoxTop"); %>
	<table cellpadding=0 cellspacing=0 border=0 class=grid>
		<thead>
			<tr>
				<th>Fund Name</th>
				<th>Total Commitment</th>
				<th>Unfunded Amount</th>
				<th>Investor Type</th>
			</tr>
		</thead>
		<tbody>
		{{each(i,fund) FundInformations}}
			<tr {{if i%2==0}}class="row"{{else}}class="arow"{{/if}}>
				<td>${fund.FundName}</td>
				<td>${formatCurrency(fund.TotalCommitment)}</td>
				<td>${formatCurrency(fund.UnfundedAmount)}</td>
				<td>${fund.InvestorType}</td>
			</tr>
		{{/each}}
		</tbody>
	</table>
	<% Html.RenderPartial("TBoxBottom"); %>
	</div>
	{{/if}}
</div>
<div style="clear: both; height: 10px">
	&nbsp;</div>
<div class="editor-button" style="width: 210px;">
	<div style="float: left; padding: 0 0 10px 5px;">
		<%: Html.Image("delete_active.png", new { @id = "Delete", @style = "cursor:pointer", @onclick = "javascript:editInvestor.deleteInvestor(this,${InvestorId});" })%>
		<%: Html.Span("", new { @id = "DeleteLoading" })%>
	</div>
</div>
