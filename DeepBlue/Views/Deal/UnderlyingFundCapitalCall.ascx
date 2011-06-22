<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundCapitalCallModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UFCC_${Index}" {{if UnderlyingFundCapitalCallId>0==false }}class="newrow"{{/if}}>
	<td style="text-align: center">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.Hidden("${Index}_FundId", "${FundId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_Amount", "{{if Amount>0}}${Amount}{{/if}}", new { @class = "", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_NoticeDate", "{{if UnderlyingFundCapitalCallId>0}}${NoticeDate}{{/if}}", new { @class = "datefield", @id = "${Index}_CC_NoticeDate" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("${Index}_ReceivedDate", "{{if UnderlyingFundCapitalCallId>0}}${ReceivedDate}{{/if}}", new { @class = "datefield", @id = "${Index}_CC_ReceivedDate" })%>
	</td>
	<td style="text-align: center">
		<%: Html.CheckBox("${Index}_IsDeemedCapitalCall", false, new { @class = "", @val = "${IsDeemedCapitalCall}" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("${Index}_UnderlyingFundCapitalCallId", "${UnderlyingFundCapitalCallId}")%>
		<%: Html.Hidden("${Index}_UnderlyingFundId", "${UnderlyingFundId}")%>
	</td>
</tr>
