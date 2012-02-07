<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="DD_${Index}" type="prow" {{if UnderlyingDirectDividendDistributionId>0==false }}class="newrow"{{/if}}>
	<td class="lalign ismanual" style="text-align:center;display:none;">
		{{if Deals.length>0}}
		<%: Html.Image("treeminus.gif", new { @onclick = "javascript:dealActivity.expandMDDTree(${Index},this);" })%>
		{{/if}}
	</td>
	<td class="lalign">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.Hidden("${Index}_FundId", "${FundId}")%>
	</td>
	<td class="lalign">
		<%: Html.TextBox("${Index}_Amount", "{{if Amount>0}}${formatNumber(Amount)}{{/if}}", new { @class = "", @onkeydown = "return jHelper.isCurrency(event);" })%>
	</td>
	<td class="lalign">
		<%: Html.TextBox("${Index}_DistributionDate", "{{if UnderlyingDirectDividendDistributionId>0}}${DistributionDate}{{/if}}", new { @class = "datefield", @id = "${Index}_DD_DistributionDate" })%>
	</td>
	<td class="ralign ismanual" style="text-align: right;display:none;">
		<%: Html.Span("${TotalCommitmentAmount}", new { @class = "money" })%>
		<%: Html.Hidden("${Index}_UnderlyingDirectDividendDistributionId", "${UnderlyingDirectDividendDistributionId}")%>
		<%: Html.Hidden("${Index}_SecurityID", "${SecurityID}")%>
		<%: Html.Hidden("${Index}_SecurityTypeID", "${SecurityTypeID}")%>
	</td><td></td>
</tr>
