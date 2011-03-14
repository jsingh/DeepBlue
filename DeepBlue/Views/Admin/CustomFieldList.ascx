<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<DeepBlue.Models.Entity.CustomField>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% FlexigridData flexData = new FlexigridData();%>
<% flexData.total = Convert.ToInt32(ViewData["TotalRows"]);
   flexData.page = Convert.ToInt32(ViewData["PageNo"]);
   FlexigridRow row;
   foreach (var item in Model) {
	   row = new FlexigridRow();
	   row.cell.Add(item.CustomFieldID.ToString());
	   row.cell.Add(item.CustomFieldText);
	   row.cell.Add(item.MODULE.ModuleName);
	   row.cell.Add(item.DataType.DataTypeName);
	   row.cell.Add(Html.Image("Edit.gif", new { @style = "cursor:pointer", @onclick = "javascript:customField.add(" + item.CustomFieldID.ToString() + ");" }).ToHtmlString() + "&nbsp;&nbsp;&nbsp;" +
				  Html.Image("Delete.png", new { @style = "cursor:pointer", @onclick = "javascript:customField.deleteEntityType(" + item.CustomFieldID.ToString() + ",this);" }).ToHtmlString());
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>
