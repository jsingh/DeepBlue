<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnfundedAdjustmentModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UFA_${DealUnderlyingFundAdjustmentId}" {{if DealUnderlyingFundAdjustmentId>0==false}}class="newrow"{{/if}}>
	<td style="text-align: center">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.Hidden("FundId", "${FundId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("CommitmentAmount", "{{if CommitmentAmount>0}}${CommitmentAmount}{{/if}}", new { @class = "", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	 <td style="text-align: center">
		<%: Html.TextBox("UnfundedAmount", "{{if UnfundedAmount>0}}${UnfundedAmount}{{/if}}", new { @class = "", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("DealUnderlyingFundAdjustmentId", "${DealUnderlyingFundAdjustmentId}")%>
		<%: Html.Hidden("DealUnderlyingFundId", "${DealUnderlyingFundId}")%>
		<%: Html.Hidden("UnderlyingFundId", "${UnderlyingFundId}")%>	
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		{{if DealUnderlyingFundAdjustmentId>0}}<%: Html.Image("largedel.png", new { @id="deletebtn", @class = "default-button", @onclick="javascript:dealActivity.deleteUFA(${DealUnderlyingFundAdjustmentId},this);" })%>
		{{/if}}
	</td>
</tr>

