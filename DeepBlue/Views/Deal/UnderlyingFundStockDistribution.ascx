<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UFSD_${Index}" {{if UnderlyingFundStockDistributionId>0==false}}class="newrow"{{/if}}>
	<td style="text-align:center;display:none;" class="ismanual">
		<%: Html.Image("treeminus.gif", new { @onclick = "javascript:dealActivity.expandMSDTree(${Index},this);" })%>
	</td>
	<td style="text-align: left">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_Issuer", "", new { @id = "Issuer" })%>
		<%: Html.Hidden("${Index}_SecurityId", "", new { @id = "SecurityId" })%>
		<%: Html.Hidden("${Index}_SecurityTypeId", "", new { @id = "SecurityTypeId" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_NumberOfShares", "", new { @onkeypress = "return jHelper.isNumeric(event);" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_FMV", "", new { @onkeypress = "return jHelper.isNumeric(event);" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_NoticeDate", "", new { @id = "${Index}_NoticeDate", @class = "datefield" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_DistributionDate", "", new { @id = "${Index}_DistributionDate", @class = "datefield" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_TaxCostBase", "", new { @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_TaxCostDate", "", new { @id = "${Index}_TaxCostDate", @class = "datefield" })%>
	</td>
	<td style="text-align: right;">
		<%: Html.Hidden("${Index}_UnderlyingFundStockDistributionId", "${UnderlyingFundStockDistributionId}")%>
		<%: Html.Hidden("${Index}_UnderlyingFundId", "${UnderlyingFundId}", new { @id = "UnderlyingFundId" })%>
		<%: Html.Hidden("${Index}_FundId", "${FundId}", new { @id = "FundId" })%>
		<%: Html.Hidden("${Index}_Index", "${Index}", new { @id = "Index" })%>
	</td>
</tr>