<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundCapitalCallModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="EmptyUFCC_${FundId}" class='emptyrow'><td colspan="6">&nbsp;</td></tr>
<tr id="UFCC_${FundId}" {{if UnderlyingFundCapitalCallId>0==false }}class="newrow"{{/if}}>
	<td style="text-align: center">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.Hidden("FundId", "${FundId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("Amount", "${Amount}", new { @class = "", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("NoticeDate", "${NoticeDate}", new { @class = "datefield", @id = "${FundId}_CC_NoticeDate" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("ReceivedDate", "${ReceivedDate}", new { @class = "datefield", @id = "${FundId}_CC_ReceivedDate" })%>
	</td>
	<td style="text-align: center">
		<%: Html.CheckBox("IsDeemedCapitalCall", false, new { @class = "", @val = "${IsDeemedCapitalCall}" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("UnderlyingFundCapitalCallId","${UnderlyingFundCapitalCallId}")%>
		<%: Html.Hidden("UnderlyingFundId", "${UnderlyingFundId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		{{if UnderlyingFundCapitalCallId>0 }}&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @id="Delete", @class = "default-button", @onclick="javascript:dealActivity.deleteCC(${UnderlyingFundCapitalCallId},this);" })%>
		{{/if}}
	</td>
</tr>
