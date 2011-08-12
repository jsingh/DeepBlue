<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundCapitalCallModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UFCC_${Index}" {{if UnderlyingFundCapitalCallId>0==false }}class="newrow"{{/if}}>
	<td class="lalign ismanual" style="text-align:center;display:none;">
		{{if Deals.length>0}}
		<%: Html.Image("treeminus.gif", new { @onclick = "javascript:dealActivity.expandMCCTree(${Index},this);" })%>
		{{/if}}
	</td>
	<td class="lalign">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.Hidden("${Index}_FundId", "${FundId}")%>
	</td>
	<td class="ralign">
		<%: Html.TextBox("${Index}_Amount", "{{if Amount>0}}${Amount}{{/if}}", new { @class = "", @onkeydown = "return jHelper.isCurrency(event);" })%>
	</td>
	<td class="lalign">
		<%: Html.TextBox("${Index}_NoticeDate", "{{if UnderlyingFundCapitalCallId>0}}${NoticeDate}{{/if}}", new { @class = "datefield", @id = "${Index}_CC_NoticeDate" })%>
	</td>
	<td class="lalign">
		<%: Html.TextBox("${Index}_ReceivedDate", "{{if UnderlyingFundCapitalCallId>0}}${ReceivedDate}{{/if}}", new { @class = "datefield", @id = "${Index}_CC_ReceivedDate" })%>
	</td>
	<td class="lalign">
		<%: Html.CheckBox("${Index}_IsDeemedCapitalCall", false, new { @class = "", @val = "${IsDeemedCapitalCall}" })%>
	</td>
	<td class="ralign ismanual" style="text-align: right;display:none;">
		<%: Html.Span("${TotalCommitmentAmount}", new { @class = "money" })%>
		<%: Html.Hidden("${Index}_UnderlyingFundCapitalCallId", "${UnderlyingFundCapitalCallId}")%>
		<%: Html.Hidden("${Index}_UnderlyingFundId", "${UnderlyingFundId}")%>
	</td><td></td>
</tr>
