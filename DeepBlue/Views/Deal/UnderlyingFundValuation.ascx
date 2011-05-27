<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundValuationModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="EmptyUFV_${UnderlyingFundNAVId}" class='emptyrow'><td colspan="6">&nbsp;</td></tr>
<tr id="UFV_${UnderlyingFundNAVId}" {{if UnderlyingFundNAVId==0}}class="newrow"{{/if}}>
	<td style="text-align: center">
		<%: Html.Span("${UnderlyingFundName}", new { @class = "show" })%>
		<%: Html.TextBox("UnderlyingFundName", "${UnderlyingFundName}", new { @class = "hide" })%>
		<%: Html.Hidden("UnderlyingFundId", "${UnderlyingFundId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${FundName}", new { @class = "show" })%>
		<%: Html.TextBox("FundName", "${FundName}", new { @class = "hide" })%>
		<%: Html.Hidden("FundId", "${FundId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${FundNAV}", new { @class = "money", @val = "{{if FundNAV > 0}}${FundNAV}{{/if}}" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${FundNAVDate}", new { @class = "dispdate", @val = "${FundNAVDate}" })%>
	</td>
	 <td style="text-align: center">
		<%: Html.Span("${CalculateNAV}", new { @class = "money", @val = "${CalculateNAV}" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${UpdateNAV}", new { @class = "show money", @val = "{{if UpdateNAV > 0}}${UpdateNAV}{{/if}}" })%>
		<%: Html.TextBox("UpdateNAV", "{{if UpdateNAV > 0}}${UpdateNAV}{{/if}}", new { @class = "hide", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${UpdateDate}", new { @class = "show dispdate", @val = "${UpdateDate}" })%>
		<%: Html.TextBox("UpdateDate", "${UpdateDate}", new { @class = "datefield hide", @id = "${UnderlyingFundNAVId}_UFV_UpdateDate" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("UnderlyingFundNAVId","${UnderlyingFundNAVId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		<%: Html.Image("tick.png", new { @id = "add", @class = "default-button {{if UnderlyingFundNAVId>0}}hide{{/if}}", @onclick = "javascript:dealActivity.addUFV(this,${UnderlyingFundNAVId});" })%>
		{{if UnderlyingFundNAVId>0}} &nbsp;&nbsp;<%: Html.Image("Edit.png", new { @class = "default-button show", @onclick = "javascript:dealActivity.editUFV(this,${UnderlyingFundNAVId});" })%>&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @class = "default-button", @onclick="javascript:dealActivity.deleteUFV(${UnderlyingFundNAVId},this);" })%>
		{{/if}}
	</td>
</tr>
