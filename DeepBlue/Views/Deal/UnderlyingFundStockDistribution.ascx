<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UFSD_${Index}" {{if UnderlyingFundStockDistributionId>0==false}}class="newrow"{{/if}}>
	<td style="text-align:center;display:none;" class="lalign ismanual">
		<%: Html.Image("treeminus.gif", new { @onclick = "javascript:dealActivity.expandMSDTree(${Index},this);" })%>
	</td>
	<td class="lalign">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
	</td>
	<td class="lalign">
		<%: Html.TextBox("${Index}_Issuer", "", new { @style="width:100px", @id = "Issuer" })%>
		<%: Html.Hidden("${Index}_SecurityId", "", new { @id = "SecurityId" })%>
		<%: Html.Hidden("${Index}_SecurityTypeId", "", new { @id = "SecurityTypeId" })%>
	</td>
	<td class="ralign">
		<%: Html.TextBox("${Index}_NumberOfShares", "", new { @style="width:100px",  @onkeypress = "return jHelper.isNumeric(event);" })%>
	</td>
	<td class="ralign">
		<%: Html.TextBox("${Index}_PurchasePrice", "", new { @style="width:100px", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td  class="lalign">
		<%: Html.TextBox("${Index}_NoticeDate", "", new { @style="width:100px", @id = "${Index}_NoticeDate", @class = "datefield" })%>
	</td>
	<td class="lalign">
		<%: Html.TextBox("${Index}_DistributionDate", "", new { @style="width:100px", @id = "${Index}_DistributionDate", @class = "datefield" })%>
	</td>
	<td class="ralign">
		<%: Html.TextBox("${Index}_TaxCostBase", "", new { @style="width:100px", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td class="lalign">
		<%: Html.TextBox("${Index}_TaxCostDate", "", new { @style="width:100px", @id = "${Index}_TaxCostDate", @class = "datefield" })%>
	</td>
	<td style="text-align: right;display:none;" class="ralign">
		<%: Html.Hidden("${Index}_UnderlyingFundStockDistributionId", "${UnderlyingFundStockDistributionId}")%>
		<%: Html.Hidden("${Index}_UnderlyingFundId", "${UnderlyingFundId}", new { @id = "UnderlyingFundId" })%>
		<%: Html.Hidden("${Index}_FundId", "${FundId}", new { @id = "FundId" })%>
		<%: Html.Hidden("${Index}_Index", "${Index}", new { @id = "Index" })%>
	</td><td></td>
</tr>