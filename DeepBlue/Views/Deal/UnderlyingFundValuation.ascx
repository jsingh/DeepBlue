<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.UnderlyingFundValuationModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="UFV_${FundId}" {{if UnderlyingFundNAVId==0}}class="newrow"{{/if}}>
	<td class="lalign">
		<%: Html.Span("${FundName}")%>
		<%: Html.Hidden("FundId", "${FundId}")%>
	</td>
	<td class="ralign">
		<%: Html.Span("${FundNAV}", new { @class = "money", @val = "{{if FundNAV > 0}}${FundNAV}{{/if}}" })%>
	</td>
	<td class="lalign">
		<%: Html.Span("${FundNAVDate}", new { @class = "dispdate", @val = "${FundNAVDate}" })%>
	</td>
	 <td class="ralign">
		<%: Html.Span("${CalculateNAV}", new { @class = "money", @val = "{{if CalculateNAV > 0}}${CalculateNAV}{{/if}}" })%>
	</td>
	<td class="ralign">
		<%: Html.Span("${UpdateNAV}", new { @class = "{{if UnderlyingFundNAVId==0}}hide{{else}}show{{/if}} money", @val = "{{if UnderlyingFundNAVId==0}}${CalculateNAV}{{else}}${UpdateNAV}{{/if}}" })%>
		<%: Html.TextBox("UpdateNAV", "{{if UnderlyingFundNAVId==0}}{{if CalculateNAV>0}}${CalculateNAV}{{/if}}{{else}}{{if UpdateNAV>0}}${UpdateNAV}{{/if}}{{/if}}", new { @class = "{{if UnderlyingFundNAVId==0}}show{{else}}hide{{/if}}", @onkeydown = "return jHelper.isCurrency(event);" })%>
	</td>
	<td class="lalign">
		<%: Html.Span("${UpdateDate}", new { @class = "{{if UnderlyingFundNAVId==0}}hide{{else}}show{{/if}} dispdate", @val = "${UpdateDate}" })%>
		<%: Html.TextBox("UpdateDate", "${UpdateDate}", new { @class = "datefield {{if UnderlyingFundNAVId==0}}show{{else}}hide{{/if}}", @id = "${UnderlyingFundNAVId}_UFV_UpdateDate" })%>
	</td>
	<td class="lalign">
		<%: Html.Span("${EffectiveDate}", new { @class = "{{if UnderlyingFundNAVId==0}}hide{{else}}show{{/if}} dispdate", @val = "${EffectiveDate}" })%>
		<%: Html.TextBox("EffectiveDate", "${EffectiveDate}", new { @class = "datefield {{if UnderlyingFundNAVId==0}}show{{else}}hide{{/if}}", @id = "${UnderlyingFundNAVId}_UFV_EffectiveDate" })%>
	</td>
	<td class="ralign">
		<%: Html.Hidden("UnderlyingFundNAVId","${UnderlyingFundNAVId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		<%: Html.Image("Save_active.png", new { @id = "add", @class = "default-button {{if UnderlyingFundNAVId>0}}hide{{/if}}", @onclick = "javascript:dealActivity.addUFV(this,${UnderlyingFundNAVId});" })%>
		{{if UnderlyingFundNAVId>0}} &nbsp;&nbsp;<%: Html.Image("Edit.png", new { @class = "default-button gbutton show", @onclick = "javascript:dealActivity.editUFV(this,${UnderlyingFundNAVId});" })%>&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @class = "default-button gbutton", @onclick = "javascript:dealActivity.deleteUFV(${FundId},${UnderlyingFundNAVId},this);" })%>
		{{/if}}
	</td>
</tr>
