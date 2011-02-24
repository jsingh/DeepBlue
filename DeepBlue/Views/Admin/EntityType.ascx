<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<DeepBlue.Models.Entity.InvestorEntityType>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="admin-list-header">
	<%: Html.ImageLink("add_icon.png")%>
</div>
<div class="admin-list">
	<table cellpadding="0" cellspacing="2" border="0" class="grid-list">
		<tr class="grid-header">
			<th style="width: 10%">
				ID
			</th>
			<th>
				Entity Type
			</th>
			<th style="width: 10%">
			</th>
		</tr>

		<% int rowIndex = 0;
	 foreach (var item in Model) { %>

				<%if (rowIndex % 2 == 0) { %>
				<tr class="row">
				<%} else {%>
				<tr class="alter-row">
				<%}%>


				<td style="text-align: center">
					<%: item.InvestorEntityTypeID %>
				</td>
				<td>
					<%: item.InvestorEntityTypeName %>
				</td>
				<td>
					<%: Html.Anchor("Edit","#",new {}) %>
					|
					<%: Html.Anchor("Delete","#",new {}) %>
				</td>
			</tr>

			<% rowIndex++;
	 } %>
			<%if (Model.Count == 0) {  %>
			<tr>
				<td colspan="3" style="text-align: center">
					No Records Found
				</td>
			</tr>
			<%} %>
	</table>
</div>
