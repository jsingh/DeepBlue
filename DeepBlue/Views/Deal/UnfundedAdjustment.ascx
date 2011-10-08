<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnfundedAdjustmentModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UFA_${DealUnderlyingFundId}" {{if DealUnderlyingFundId>0==false}}class="newrow"{{/if}}>
	<td class="lalign">
		<%: Html.Span("${FundName}")%>
	</td>
	<td class="ralign">
		<%: Html.Span("${CommitmentAmount}", new { @class = "money" })%>
	</td>
	 <td class="ralign">
		<%: Html.Span("${UnfundedAmount}", new { @class = "money" })%>
	</td>
	<td class="lalign">
		<%: Html.TextBox("CommitmentAmount", "{{if CommitmentAmount>0}}${CommitmentAmount}{{/if}}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
	</td>
	<td class="lalign">
		<%: Html.TextBox("UnfundedAmount", "{{if UnfundedAmount>0}}${UnfundedAmount}{{/if}}", new { @class = "hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
	</td>
	<td class="ralign">
		<%: Html.Hidden("DealUnderlyingFundId", "${DealUnderlyingFundId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		<%: Html.Image("Save_active.png", new { @id = "add", @class = "default-button {{if DealUnderlyingFundId>0}}hide{{/if}}", @onclick = "javascript:dealActivity.addUFA(this,${DealUnderlyingFundId});" })%>
		<%: Html.Image("Edit.png", new { @class = "default-button show gbutton", @onclick = "javascript:dealActivity.editUFA(this,${DealUnderlyingFundId});" })%>
	</td>
</tr>

