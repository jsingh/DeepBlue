<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingDirectValuationModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="EmptyUDV_${UnderlyingDirectLastPriceId}" class='emptyrow'><td colspan="6">&nbsp;</td></tr>
<tr id="UDV_${UnderlyingDirectLastPriceId}" {{if UnderlyingDirectLastPriceId==0}}class="newrow"{{/if}}>
	<td style="text-align: center">
		<%: Html.Span("${FundName}")%>
		<%: Html.Hidden("FundId", "${FundId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${Security}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${SecurityType}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${LastPrice}", new { @id="LastPrice", @class = "money", @val = "${LastPrice}" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${LastPriceDate}", new { @id = "LastPriceDate", @class = "dispdate", @val = "${LastPriceDate}" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("", new { @class = "{{if UnderlyingDirectLastPriceId>0}}show{{else}}hide{{/if}}"})%>
		<%: Html.TextBox("NewPrice", "", new { @class = "{{if UnderlyingDirectLastPriceId>0}}hide{{else}}show{{/if}}", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("", new { @class = "{{if UnderlyingDirectLastPriceId>0}}show{{else}}hide{{/if}} dispdate", @val = "${NewPriceDate}" })%>
		<%: Html.TextBox("NewPriceDate", "", new { @class = "datefield {{if UnderlyingDirectLastPriceId>0}}hide{{else}}show{{/if}}", @id = "${UnderlyingDirectNewPriceId}_UDV_NewPriceDate" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("SecurityTypeId", "${SecurityTypeId}")%>
		<%: Html.Hidden("SecurityId", "${SecurityId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		<%: Html.Image("tick.png", new { @id = "add", @class = "default-button {{if UnderlyingDirectLastPriceId>0}}hide{{/if}}", @onclick = "javascript:dealActivity.addUDV(this,${UnderlyingDirectLastPriceId});" })%>
		{{if UnderlyingDirectLastPriceId>0}} &nbsp;&nbsp;<%: Html.Image("Edit.png", new { @id = "editbtn", @class = "default-button show", @onclick = "javascript:dealActivity.editUDV(this,${UnderlyingDirectLastPriceId});" })%>&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @id="deletebtn", @class = "default-button", @onclick="javascript:dealActivity.deleteUDV(${UnderlyingDirectLastPriceId},this);" })%>
		{{/if}}
	</td>
</tr>
