<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnfundedAdjustmentModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UFA_${DealUnderlyingFundId}" {{if DealUnderlyingFundId>0==false}}class="newrow"{{/if}}>
	<td style="text-align: left;">
		<%: Html.Span("${FundName}")%>
	</td>
	<td style="text-align: right">
		<%: Html.Span("${CommitmentAmount}", new { @class = "money" })%>
	</td>
	 <td style="text-align: right">
		<%: Html.Span("${UnfundedAmount}", new { @class = "money" })%>
	</td>
	<td style="text-align: right">
		<%: Html.TextBox("CommitmentAmount", "{{if CommitmentAmount>0}}${CommitmentAmount}{{/if}}", new { @class = "hide", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: right">
		<%: Html.TextBox("UnfundedAmount", "{{if UnfundedAmount>0}}${UnfundedAmount}{{/if}}", new { @class = "hide", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("DealUnderlyingFundId", "${DealUnderlyingFundId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		<%: Html.Image("save.png", new { @id = "add", @class = "default-button {{if DealUnderlyingFundId>0}}hide{{/if}}", @onclick = "javascript:dealActivity.addUFA(this,${DealUnderlyingFundId});" })%>
		<%: Html.Image("Edit.png", new { @class = "default-button show gbutton", @onclick = "javascript:dealActivity.editUFA(this,${DealUnderlyingFundId});" })%>
	</td>
</tr>

