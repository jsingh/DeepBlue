<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundPostRecordCapitalCallModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="EmptyUFPRCC_${UnderlyingFundCapitalCallLineItemId}" class='emptyrow'><td colspan="6">&nbsp;</td></tr>
<tr id="UFPRCC_${UnderlyingFundCapitalCallLineItemId}" {{if UnderlyingFundCapitalCallLineItemId==0}}class="newrow"{{/if}}>
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
		<%: Html.Span("${ReceivedDate}", new { @class = "show dispdate", @val = "${ReceivedDate}" })%>
		<%: Html.TextBox("ReceivedDate", "${ReceivedDate}", new { @class = "datefield hide", @id = "${UnderlyingFundCapitalCallLineItemId}_PRCC_ReceivedDate" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("UnderlyingFundCapitalCallLineItemId","${UnderlyingFundCapitalCallLineItemId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		<%: Html.Image("tick.png", new { @id = "add", @class = "default-button {{if UnderlyingFundCapitalCallLineItemId>0}}hide{{/if}}", @onclick = "javascript:dealActivity.addPRCC(this,${UnderlyingFundCapitalCallLineItemId});" })%>
		{{if UnderlyingFundCapitalCallLineItemId>0}} &nbsp;&nbsp;<%: Html.Image("Edit.png", new { @class = "default-button show", @onclick = "javascript:dealActivity.editPRCC(this,${UnderlyingFundCapitalCallLineItemId});" })%>&nbsp;&nbsp;<%: Html.Image("Delete.png", new { @class = "default-button", @onclick="javascript:dealActivity.deletePRCC(${UnderlyingFundCapitalCallLineItemId},this);" })%>
		{{/if}}
	</td>
</tr>
