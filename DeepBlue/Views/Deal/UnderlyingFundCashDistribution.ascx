<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundCashDistributionModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="EmptyUFCD_${UnderlyingFundCashDistributionId}" class='emptyrow'><td colspan="6">&nbsp;</td></tr>
<tr id="UFCD_${UnderlyingFundCashDistributionId}" {{if UnderlyingFundCashDistributionId>0==false}}class="newrow"{{/if}}>
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
		<%: Html.DropDownList("CashDistributionTypeId", Model.CashDistributionTypes, new { @style = "width:160px", @val = "${CashDistributionTypeId}" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("UnderlyingFundCashDistributionId","${UnderlyingFundCashDistributionId}")%>
		<%: Html.Hidden("UnderlyingFundId", "${UnderlyingFundId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		{{if UnderlyingFundCashDistributionId>0}}&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @id="Delete", @class = "default-button", @onclick="javascript:dealActivity.deleteCD(${UnderlyingFundCashDistributionId},this);" })%>
		{{/if}}
	</td>
</tr>