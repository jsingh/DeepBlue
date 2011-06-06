<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundPostRecordCapitalCallModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="EmptyUFPRCC_${FundId}" class='emptyrow'><td colspan="6">&nbsp;</td></tr>
<tr id="UFPRCC_${FundId}" {{if UnderlyingFundCapitalCallLineItemId>0==false}}class="newrow"{{/if}}>
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
		<%: Html.TextBox("ReceivedDate", "${ReceivedDate}", new { @class = "datefield", @id = "${FundId}_PRCC_ReceivedDate" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("UnderlyingFundCapitalCallLineItemId","${UnderlyingFundCapitalCallLineItemId}")%>
		<%: Html.Hidden("UnderlyingFundId", "${UnderlyingFundId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		{{if UnderlyingFundCapitalCallLineItemId>0}}&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @id="Delete", @class = "default-button", @onclick="javascript:dealActivity.deletePRCC(${UnderlyingFundCapitalCallLineItemId},this);" })%>
		{{/if}}
	</td>
</tr>
