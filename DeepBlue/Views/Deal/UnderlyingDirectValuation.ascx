<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingDirectValuationModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UDV_${UnderlyingDirectLastPriceId}" {{if UnderlyingDirectLastPriceId==0}}class="newrow"{{/if}}>
	<td class="lalign">
		<%: Html.Span("${DirectName}")%>
	</td>
	<td class="lalign">
		<%: Html.Span("${FundName}")%>
		<%: Html.Hidden("FundId", "${FundId}")%>
	</td>
	<td class="ralign">
		<%: Html.Span("${LastPrice}", new { @id="LastPrice", @class = "money", @val = "${LastPrice}" })%>
	</td>
	<td class="lalign">
		<%: Html.Span("${LastPriceDate}", new { @id = "LastPriceDate", @class = "dispdate", @val = "${LastPriceDate}" })%>
	</td>
	<td class="ralign">
		<%: Html.TextBox("NewPrice", "{{if NewPrice > 0}}${NewPrice}{{/if}}", new {  @onkeydown = "return jHelper.isCurrency(event);" })%>
	</td>
	<td class="lalign">
		<%: Html.TextBox("NewPriceDate", "${NewPriceDate}", new { @class = "datefield", @id = "${UnderlyingDirectLastPriceId}_UDV_NewPriceDate" })%>
	</td>
	<td class="ralign">
		<%: Html.Hidden("SecurityTypeId", "${SecurityTypeId}")%>
		<%: Html.Hidden("SecurityId", "${SecurityId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		{{if UnderlyingDirectLastPriceId>0}}<%: Html.Image("largedel.png", new { @id="deletebtn", @class = "default-button gbutton", @onclick="javascript:dealActivity.deleteUDV(${UnderlyingDirectLastPriceId},this);" })%>
		{{/if}}
	</td>
</tr>
