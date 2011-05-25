<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundCashDistributionModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="EmptyUFCD_${UnderlyingFundCashDistributionId}" class='emptyrow'><td colspan="6">&nbsp;</td></tr>
<tr id="UFCD_${UnderlyingFundCashDistributionId}" {{if UnderlyingFundCashDistributionId==0}}class="newrow"{{/if}}>
	<td style="text-align: center">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.TextBox("FundName", "${FundName}", new { @class = "hide" })%>
		<%: Html.Hidden("FundId", "${FundId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${CashDistributionType}", new { @class = "show" })%>
		<%: Html.DropDownList("CashDistributionTypeId", Model.CashDistributionTypes, new { @class = "hide", @val="${CashDistributionTypeId}" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${Amount}", new { @class = "show money", @val = "${Amount}" })%>
		<%: Html.TextBox("Amount", "${Amount}", new { @class = "hide", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${NoticeDate}", new { @class = "show dispdate", @val = "${NoticeDate}" })%>
		<%: Html.TextBox("NoticeDate", "${NoticeDate}", new { @class = "datefield hide", @id = "${UnderlyingFundCashDistributionId}_CC_NoticeDate" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${ReceivedDate}", new { @class = "show dispdate", @val = "${ReceivedDate}" })%>
		<%: Html.TextBox("ReceivedDate", "${ReceivedDate}", new { @class = "datefield hide", @id = "${UnderlyingFundCashDistributionId}_CC_ReceivedDate" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("{{if IsDeemedDistribution==true}}Yes{{else}}No{{/if}}", new { @class = "show", @val = "${IsDeemedDistribution}" })%>
		<%: Html.CheckBox("IsDeemedDistribution", false, new { @class = "hide", @val = "${IsDeemedDistribution}" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("{{if IsNettedDistribution==true}}Yes{{else}}No{{/if}}", new { @class = "show", @val = "${IsNettedDistribution}" })%>
		<%: Html.CheckBox("IsNettedDistribution", false, new { @class = "hide", @val = "${IsNettedDistribution}" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("UnderlyingFundCashDistributionId","${UnderlyingFundCashDistributionId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		<%: Html.Image("tick.png", new { @id = "add", @class = "default-button {{if UnderlyingFundCashDistributionId>0}}hide{{/if}}", @onclick = "javascript:dealActivity.addCD(this,${UnderlyingFundCashDistributionId});" })%>
		{{if UnderlyingFundCashDistributionId>0}} &nbsp;&nbsp;<%: Html.Image("Edit.png", new { @class = "default-button show", @onclick = "javascript:dealActivity.editCD(this,${UnderlyingFundCashDistributionId});" })%>&nbsp;&nbsp;<%: Html.Image("Delete.png", new { @class = "default-button", @onclick="javascript:dealActivity.deleteCD(${UnderlyingFundCashDistributionId},this);" })%>
		{{/if}}
	</td>
</tr>