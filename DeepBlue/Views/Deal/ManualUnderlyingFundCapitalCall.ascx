<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="ManualUFCC_${Index}" {{if UnderlyingFundCapitalCallId>0==false }}class="newrow"{{/if}}>
	<td style="text-align: left">
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
<tr id="ManualUFCC_Deal_${Index}" {{if UnderlyingFundCapitalCallId>0==false }}class="newrow"{{/if}} style="background-color:transparent;">
	<td colspan=6 style="padding-left:20px">
		<table cellpadding=0 cellspacing=0 border=0 style="width:75%" class="grid">
			<thead>
				<tr>
					<th>Deal Number</th>
					<th>Deal Name</th>
					<th>Commitment Amount</th>
					<th>Call Amount</th>
				</tr>
			</thead>
			{{each(i,deal) Deals}}
				<tr>
					<td>${deal.DealNumber}</td>
					<td>${deal.DealName}</td>
					<td>${deal.CommitmentAmount}</td>
					<td>${deal.CallAmount}</td>
				</tr>
			{{/each}}
		</table>
	</td>
</tr>