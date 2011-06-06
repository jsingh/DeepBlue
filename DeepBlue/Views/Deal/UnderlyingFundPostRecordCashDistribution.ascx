<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundPostRecordCashDistributionModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="EmptyUFPRCD_${DealId}" class='emptyrow'><td colspan="6">&nbsp;</td></tr>
<tr id="UFPRCD_${DealId}" {{if CashDistributionId>0==false}}class="newrow"{{/if}}>
	<td style="text-align: center">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.Hidden("FundId", "${FundId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${DealName}", new { @class = "show" })%>
		<%: Html.Hidden("DealId", "${DealId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("Amount", "${Amount}", new { @class = "", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("DistributionDate", "${DistributionDate}", new { @class = "datefield", @id = "${FundId}_PRCD_DistributionDate" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("CashDistributionId","${CashDistributionId}")%>
		<%: Html.Hidden("UnderlyingFundId", "${UnderlyingFundId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		{{if CashDistributionId>0}}&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @id="Delete", @class = "default-button", @onclick="javascript:dealActivity.deletePRCD(${CashDistributionId},this);" })%>
		{{/if}}
	</td>
</tr>
