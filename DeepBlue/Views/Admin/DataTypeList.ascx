<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<DeepBlue.Models.Entity.DataType>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% FlexigridData flexData = new FlexigridData();%>
<% flexData.total = Convert.ToInt32(ViewData["TotalRows"]);
   flexData.page = Convert.ToInt32(ViewData["PageNo"]);
   FlexigridRow row;
   foreach (var item in Model) {
	   row = new FlexigridRow();
	   row.cell.Add(item.DataTypeID.ToString());
	   row.cell.Add(item.DataTypeName);
	   row.cell.Add(Html.Image("Edit.gif", new {  @onclick = "javascript:dataType.add(" + item.DataTypeID.ToString() + ");" }).ToHtmlString());
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>
