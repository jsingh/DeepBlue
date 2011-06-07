<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingDirectValuationModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="EmptyUDV_${UnderlyingDirectLastPriceId}" class='emptyrow'><td colspan="6">&nbsp;</td></tr>
<tr id="UDV_${UnderlyingDirectLastPriceId}" {{if UnderlyingDirectLastPriceId==0}}class="newrow"{{/if}}>
	<td style="text-align: center">
		<%: Html.Span("${DirectName}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${FundName}")%>
		<%: Html.Hidden("FundId", "${FundId}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${LastPrice}", new { @id="LastPrice", @class = "money", @val = "${LastPrice}" })%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${LastPriceDate}", new { @id = "LastPriceDate", @class = "dispdate", @val = "${LastPriceDate}" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("NewPrice", "{{if NewPrice > 0}}${NewPrice}{{/if}}", new {  @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: center">
		<%: Html.TextBox("NewPriceDate", "${NewPriceDate}", new { @class = "datefield", @id = "${UnderlyingDirectNewPriceId}_UDV_NewPriceDate" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("SecurityTypeId", "${SecurityTypeId}")%>
		<%: Html.Hidden("SecurityId", "${SecurityId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		{{if UnderlyingDirectLastPriceId>0}}<%: Html.Image("largedel.png", new { @id="deletebtn", @class = "default-button", @onclick="javascript:dealActivity.deleteUDV(${UnderlyingDirectLastPriceId},this);" })%>
		{{/if}}
	</td>
</tr>
