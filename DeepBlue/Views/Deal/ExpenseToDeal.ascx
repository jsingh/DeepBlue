<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<tr id="ETD_${DealId}" {{if DealId==0}}class="newrow"{{/if}}>
	<td style="text-align: center">
		<%: Html.Span("${DealNo}", new { @class = "show1" })%>
		<%--<%: Html.TextBox("DealNo", "${DealNo}", new { @class = "hide" })%>--%>
		<%: Html.Hidden("DealNo", "${DealNo}")%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${DealName}", new { @class = "show1", @val = "${DealName}" })%>
		<%--<%: Html.TextBox("DealName", "${DealName}", new { @class = "hide" })%>--%>
	</td>
	<td style="text-align: center">
		<%: Html.Span("${Amount}", new { @class = "show money", @val = "${Amount}" })%>
		<%: Html.TextBox("Amount", "{{if Amount>0}}${Amount}{{/if}}", new { @class = "hide", @onkeypress = "return jHelper.isCurrency(event);" })%>
	</td>
	<td style="text-align: right">
		<%: Html.Hidden("DealId","${DealId}")%>
		<%: Html.Span("", new { id = "UpdateLoading" })%>
		<%: Html.Image("save.png", new { @id = "add", @class = "default-button {{if DealId>0}}hide{{/if}}", @onclick = "javascript:dealActivity.addETD(this,${DealId});" })%>
		{{if DealId>0}} &nbsp;&nbsp;<%: Html.Image("Edit.png", new { @class = "default-button show", @onclick = "javascript:dealActivity.editETD(this,${DealId});" })%>&nbsp;&nbsp;<%: Html.Image("largedel.png", new { @class = "default-button", @onclick="javascript:dealActivity.deleteETD(${DealId},this);" })%>
		{{/if}}
	</td>
</tr>
