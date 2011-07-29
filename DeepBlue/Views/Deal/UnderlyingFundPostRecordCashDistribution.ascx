<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundPostRecordCashDistributionModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UFPRCD_${Index}" {{if CashDistributionId>0==false}}class="newrow"{{/if}}>
	<td class="lalign">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.Hidden("${Index}_FundId", "${FundId}")%>
	</td>
	<td class="lalign">
		<%: Html.Span("${DealName}", new { @class = "show" })%>
		<%: Html.Hidden("${Index}_DealId", "${DealId}")%>
	</td>
	<td class="ralign">
		<%: Html.TextBox("${Index}_Amount", "{{if Amount>0}}${Amount}{{/if}}", new { @class = "", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td class="lalign">
		<%: Html.TextBox("${Index}_DistributionDate", "{{if CashDistributionId>0}}${DistributionDate}{{/if}}", new { @class = "datefield", @id = "${Index}_PRCD_DistributionDate" })%>
	</td>
	<td class="ralign">
		<%: Html.Hidden("${Index}_CashDistributionId", "${CashDistributionId}")%>
		<%: Html.Hidden("${Index}_UnderlyingFundId", "${UnderlyingFundId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		{{if CashDistributionId>0}}&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @id="Delete", @class = "default-button", @onclick="javascript:dealActivity.deletePRCD(${Index},${CashDistributionId},this);" })%>
		{{/if}}
	</td>
</tr>
