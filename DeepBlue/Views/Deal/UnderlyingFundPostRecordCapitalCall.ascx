<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundPostRecordCapitalCallModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UFPRCC_${Index}" {{if UnderlyingFundCapitalCallLineItemId>0==false}}class="newrow"{{/if}}>
	<td class="lalign">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.Hidden("${Index}_FundId", "${FundId}")%>
	</td>
	<td class="lalign">
		<%: Html.Span("${DealName}", new { @class = "show" })%>
		<%: Html.Hidden("${Index}_DealId", "${DealId}")%>
	</td>
	<td class="ralign">
		<%: Html.TextBox("${Index}_Amount", "{{if Amount>0}}${Amount}{{/if}}", new { @class = "", @onkeydown = "return jHelper.isCurrency(event);" })%>
	</td>
	<td class="lalign">
		<%: Html.TextBox("${Index}_CapitalCallDate", "{{if UnderlyingFundCapitalCallLineItemId>0}}${CapitalCallDate}{{/if}}", new { @class = "datefield", @id = "${Index}_PRCC_CapitalCallDate" })%>
	</td>
	<td class="ralign">
		<%: Html.Hidden("${Index}_UnderlyingFundCapitalCallLineItemId", "${UnderlyingFundCapitalCallLineItemId}")%>
		<%: Html.Hidden("${Index}_UnderlyingFundId", "${UnderlyingFundId}")%>
	</td>
</tr>
