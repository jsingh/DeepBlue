<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<DeepBlue.Models.Entity.MODULE>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% FlexigridData flexData = new FlexigridData();%>
<% flexData.total = Convert.ToInt32(ViewData["TotalRows"]);
   flexData.page = Convert.ToInt32(ViewData["PageNo"]);
   FlexigridRow row;
   foreach (var item in Model) {
	   row = new FlexigridRow();
	   row.cell.Add(item.ModuleID.ToString());
	   row.cell.Add(item.ModuleName);
	   row.cell.Add(Html.Image("Edit.gif", new { @style = "cursor:pointer", @onclick = "javascript:module.add(" + item.ModuleID.ToString() + ");" }).ToHtmlString());
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>
