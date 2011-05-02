<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<DeepBlue.Models.Entity.ShareClassType>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% FlexigridData flexData = new FlexigridData();%>
<% flexData.total = Convert.ToInt32(ViewData["TotalRows"]);
   flexData.page = Convert.ToInt32(ViewData["PageNo"]);
   FlexigridRow row;
   foreach (var item in Model) {
	   row = new FlexigridRow();
	   row.cell.Add(item.ShareClassTypeID.ToString());
	   row.cell.Add(item.ShareClass);
	   row.cell.Add(item.Enabled);
	   row.cell.Add(Html.Image("Edit.gif", new { @style = "cursor:pointer", @onclick = "javascript:shareclasstype.add(" + item.ShareClassTypeID.ToString() + ");" }).ToHtmlString());
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>
