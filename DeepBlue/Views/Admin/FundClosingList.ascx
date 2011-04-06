<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<DeepBlue.Models.Entity.FundClosing>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% FlexigridData flexData = new FlexigridData();%>
<% flexData.total = Convert.ToInt32(ViewData["TotalRows"]);
   flexData.page = Convert.ToInt32(ViewData["PageNo"]);
   FlexigridRow row;
   foreach (var item in Model) {
	   row = new FlexigridRow();
	   row.cell.Add(item.FundClosingID.ToString());
	   row.cell.Add((item.FundClosingDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy"));
	   row.cell.Add(item.Name);
	   row.cell.Add(item.Fund.FundName);
	   row.cell.Add(item.IsFirstClosing);
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>
