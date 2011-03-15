<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<DeepBlue.Models.Entity.InvestorEntityType>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% FlexigridData flexData = new FlexigridData();%>
<% flexData.total = Convert.ToInt32(ViewData["TotalRows"]);
   flexData.page = Convert.ToInt32(ViewData["PageNo"]);
   FlexigridRow row;
   foreach (var item in Model) {
	   row = new FlexigridRow();
	   row.cell.Add(item.InvestorEntityTypeID.ToString());
	   row.cell.Add(item.InvestorEntityTypeName);
	   if (item.Enabled)
		   row.cell.Add(Html.Image("Tick.gif").ToHtmlString());
	   else
		   row.cell.Add(string.Empty);
	   row.cell.Add(Html.Image("Edit.gif", new {  @onclick = "javascript:invEntityType.add(" + item.InvestorEntityTypeID.ToString() + ");" }).ToHtmlString() + "&nbsp;&nbsp;&nbsp;" +
					Html.Image("Delete.png", new {  @onclick = "javascript:invEntityType.deleteEntityType(" + item.InvestorEntityTypeID.ToString() + ",this);" }).ToHtmlString());
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>
