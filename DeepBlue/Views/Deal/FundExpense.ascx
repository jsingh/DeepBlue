<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Deal.FundExpenseModel>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="FLE_${FundExpenseId}">
	<td class="lalign">
		<%: Html.Span("${FundExpenseType}", new { @class = "show" })%>
		<%: Html.DropDownList("FundExpenseTypeId", Model.FundExpenseTypes, new { @val = "${FundExpenseTypeId}", @class = "hide" })%>
	</td>
	<td class="ralign">
		<%: Html.Span("${Amount}", new { @class = "show money" })%>
		<%: Html.TextBox("Amount", "{{if Amount>0}}${Amount}{{/if}}", new { @class="hide", @onkeydown = "return jHelper.isCurrency(event);" })%>
	</td>
	<td class="lalign">
		<%: Html.Span("${Date}", new { @class = "show dispdate" })%>
		<%: Html.TextBox("Date", "${Date}", new { @class = "hide datefield", @id = "${FundExpenseId}_FE_Date" })%>
	</td>
	<td class="ralign">
		<%: Html.Hidden("FundId", "${FundId}")%>
		<%: Html.Hidden("FundExpenseId", "${FundExpenseId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		{{if FundExpenseId>0}}
		<%: Html.Image("save.png", new { @id = "save", @class = "default-button {{if FundExpenseId>0}}hide{{/if}}", @onclick = "javascript:dealActivity.addFLE(this,${FundExpenseId});" })%>
		<%: Html.Image("Edit.png", new { @class = "default-button show  gbutton", @onclick = "javascript:dealActivity.editFLE(this,${FundExpenseId});" })%>
		{{else}}
		<%: Html.Image("add.png", new { @id = "add", @class = "default-button", @onclick = "javascript:dealActivity.addFLE(this,0);" })%>
		{{/if}}
	</td>
</tr>
