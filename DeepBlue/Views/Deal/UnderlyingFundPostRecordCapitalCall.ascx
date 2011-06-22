<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundPostRecordCapitalCallModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UFPRCC_${Index}" {{if UnderlyingFundCapitalCallLineItemId>0==false}}class="newrow"{{/if}}>
	<td style="text-align: center">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.Hidden("${Index}_FundId", "${FundId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${DealName}", new { @class = "show" })%>
		<%: Html.Hidden("${Index}_DealId", "${DealId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_Amount", "{{if Amount>0}}${Amount}{{/if}}", new { @class = "", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_CapitalCallDate", "{{if UnderlyingFundCapitalCallLineItemId>0}}${CapitalCallDate}{{/if}}", new { @class = "datefield", @id = "${Index}_PRCC_CapitalCallDate" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("${Index}_UnderlyingFundCapitalCallLineItemId", "${UnderlyingFundCapitalCallLineItemId}")%>
		<%: Html.Hidden("${Index}_UnderlyingFundId", "${UnderlyingFundId}")%>
	</td>
</tr>
