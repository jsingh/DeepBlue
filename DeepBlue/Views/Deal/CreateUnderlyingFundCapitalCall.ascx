<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundCapitalCallModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr>
	<td style="display: none">
		<%: Html.Hidden("UnderlyingFundCapitalCallId","${UnderlyingFundCapitalCallId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.TextBox("FundName", "${FundName}", new { @class = "hide" })%>
		<%: Html.Hidden("FundId", "${FundId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${Amount}", new { @class = "show", @val = "${Amount}" })%>
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
		<%: Html.Span("{{if IsDeemedCapitalCall == 'true'}}Yes{{else}}No{{/if}}", new { @class = "show", @val = "${IsDeemedCapitalCall}" })%>
		<%: Html.CheckBox("IsDeemedCapitalCall", false, new { @class = "hide", @val = "${IsDeemedCapitalCall}" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Hidden("UnderlyingFundId","${UnderlyingFundId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		{{if UnderlyingFundCapitalCallId>0}}
		<%: Html.Image("Edit.png", new { @class = "default-button" })%>
		{{else}}
		<%: Html.Image("tick.png", new { @class = "default-button" })%>
		{{/if}}
	</td>
</tr>
