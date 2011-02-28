<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<DeepBlue.Models.Entity.InvestorEntityType>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<div class="admin-list-header">
	<%: Html.Image("add_icon.png")%>
</div>
<div class="admin-list">
	<table id="EntityTypes">
		<thead>
			<tr>
				<th calign="center" style="width: 100px;text-align:center;" align="center">
					ID
				</th>
				<th style="width: 600px">
					Entity Type
				</th>
				<th calign="center" style="width: 100px">
				</th>
			</tr>
		</thead>
		<tbody>
			<% foreach (var item in Model) { %>
			<tr>
				<td style="width: 100px">
					<%: item.InvestorEntityTypeID %>
				</td>
				<td style="width: 600px">
					<%: item.InvestorEntityTypeName %>
				</td>
				<td style="width: 100px">
					<%: Html.Anchor("Edit","#",new {}) %>
					|
					<%: Html.Anchor("Delete","#",new {}) %>
				</td>
			</tr>
			<%} %>
		</tbody>
	</table>
</div>
