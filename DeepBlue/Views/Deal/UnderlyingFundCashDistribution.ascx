<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundCashDistributionModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UFCD_${Index}" {{if UnderlyingFundCashDistributionId>0==false}}class="newrow"{{/if}}>
	<td style="text-align:center;display:none;" class="calign ismanual">
		{{if Deals.length>0}}
		<%: Html.Image("treeminus.gif", new { @onclick = "javascript:dealActivity.expandMCDTree(${Index},this);" })%>
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
		<%: Html.TextBox("${Index}_NoticeDate", "{{if UnderlyingFundCashDistributionId>0}}${NoticeDate}{{/if}}", new { @class = "datefield", @id = "${Index}_CD_NoticeDate" })%>
	</td>
	<td class="lalign">
		<%: Html.DropDownList("${Index}_CashDistributionTypeId", Model.CashDistributionTypes, new {   @val = "${CashDistributionTypeId}" })%>
	</td>
	<td style="display:none;" class="ralign ismanual">
		<%: Html.Span("${TotalCommitmentAmount}", new { @class = "money" })%>
		<%: Html.Hidden("${Index}_UnderlyingFundCashDistributionId", "${UnderlyingFundCashDistributionId}")%>
		<%: Html.Hidden("${Index}_UnderlyingFundId", "${UnderlyingFundId}")%>
	</td><td></td>
</tr>