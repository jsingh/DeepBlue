<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="PRDD_${Index}" {{if DividendDistributionID>0==false}}class="newrow"{{/if}}>
	<td class="lalign">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.Hidden("${Index}_FundId", "${FundId}")%>
	</td>
	<td class="lalign">
		<%: Html.Span("${DealName}", new { @class = "show" })%>
		<%: Html.Hidden("${Index}_DealId", "${DealId}")%>
	</td>
	<td class="lalign">
		<%: Html.TextBox("${Index}_Amount", "{{if Amount>0}}${Amount}{{/if}}", new { @class = "", @onkeydown = "return jHelper.isCurrency(event);" })%>
	</td>
	<td class="lalign">
		<%: Html.TextBox("${Index}_DistributionDate", "{{if DividendDistributionID>0}}${DistributionDate}{{/if}}", new { @class = "datefield", @id = "${Index}_PRCC_DistributionDate" })%>
	</td>
	<td class="ralign">
		<%: Html.Hidden("${Index}_DividendDistributionID", "${DividendDistributionID}")%>
		<%: Html.Hidden("${Index}_SecurityID", "${SecurityID}")%>
		<%: Html.Hidden("${Index}_SecurityTypeID", "${SecurityTypeID}")%>
		<%: Html.Hidden("${Index}_Id", "${Id}")%>
	</td>
</tr>
