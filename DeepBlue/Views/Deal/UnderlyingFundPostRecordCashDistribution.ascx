<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundPostRecordCashDistributionModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="EmptyUFPRCD_${CashDistributionId}" class='emptyrow'><td colspan="6">&nbsp;</td></tr>
<tr id="UFPRCD_${CashDistributionId}" {{if CashDistributionId==0}}class="newrow"{{/if}}>
	<td style="text-align: center">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.TextBox("FundName", "${FundName}", new { @class = "hide" })%>
		<%: Html.Hidden("FundId", "${FundId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${DealName}", new { @class = "show" })%>
		<%: Html.TextBox("DealName", "${DealName}", new { @class = "hide" })%>
		<%: Html.Hidden("DealId", "${DealId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${Amount}", new { @class = "show money", @val = "${Amount}" })%>
		<%: Html.TextBox("Amount", "${Amount}", new { @class = "hide", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${DistributionDate}", new { @class = "show dispdate", @val = "${DistributionDate}" })%>
		<%: Html.TextBox("DistributionDate", "${DistributionDate}", new { @class = "datefield hide", @id = "${CashDistributionId}_PRCD_DistributionDate" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("CashDistributionId","${CashDistributionId}")%>
		<%: Html.Hidden("UnderlyingFundId", "${UnderlyingFundId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		<%: Html.Image("tick.png", new { @id = "add", @class = "default-button {{if CashDistributionId>0}}hide{{/if}}", @onclick = "javascript:dealActivity.addPRCD(this,${CashDistributionId});" })%>
		{{if CashDistributionId>0}} &nbsp;&nbsp;<%: Html.Image("Edit.png", new { @class = "default-button show", @onclick = "javascript:dealActivity.editPRCD(this,${CashDistributionId});" })%>&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @class = "default-button", @onclick="javascript:dealActivity.deletePRCD(${CashDistributionId},this);" })%>
		{{/if}}
	</td>
</tr>
