<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundCapitalCallModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="EmptyUFCC_${UnderlyingFundCapitalCallId}" class='emptyrow'><td colspan="6">&nbsp;</td></tr>
<tr id="UFCC_${UnderlyingFundCapitalCallId}" {{if UnderlyingFundCapitalCallId==0}}class="newrow"{{/if}}>
	<td style="text-align: center">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.TextBox("FundName", "${FundName}", new { @class = "hide" })%>
		<%: Html.Hidden("FundId", "${FundId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${Amount}", new { @class = "show money", @val = "${Amount}" })%>
		<%: Html.TextBox("Amount", "${Amount}", new { @class = "hide", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${NoticeDate}", new { @class = "show dispdate", @val = "${NoticeDate}" })%>
		<%: Html.TextBox("NoticeDate", "${NoticeDate}", new { @class = "datefield hide", @id = "${UnderlyingFundCapitalCallId}_CC_NoticeDate" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${ReceivedDate}", new { @class = "show dispdate", @val = "${ReceivedDate}" })%>
		<%: Html.TextBox("ReceivedDate", "${ReceivedDate}", new { @class = "datefield hide", @id = "${UnderlyingFundCapitalCallId}_CC_ReceivedDate" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("{{if IsDeemedCapitalCall==true}}Yes{{else}}No{{/if}}", new { @class = "show", @val = "${IsDeemedCapitalCall}" })%>
		<%: Html.CheckBox("IsDeemedCapitalCall", false, new { @class = "hide", @val = "${IsDeemedCapitalCall}" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("UnderlyingFundCapitalCallId","${UnderlyingFundCapitalCallId}")%>
		<%: Html.Hidden("UnderlyingFundId", "${UnderlyingFundId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		<%: Html.Image("tick.png", new { @id = "add", @class = "default-button {{if UnderlyingFundCapitalCallId>0}}hide{{/if}}", @onclick = "javascript:dealActivity.addCC(this,${UnderlyingFundCapitalCallId});" })%>
		{{if UnderlyingFundCapitalCallId>0}} &nbsp;&nbsp;<%: Html.Image("Edit.png", new { @class = "default-button show", @onclick = "javascript:dealActivity.editCC(this,${UnderlyingFundCapitalCallId});" })%>&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @class = "default-button", @onclick="javascript:dealActivity.deleteCC(${UnderlyingFundCapitalCallId},this);" })%>
		{{/if}}
	</td>
</tr>
