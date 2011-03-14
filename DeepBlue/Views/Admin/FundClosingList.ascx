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
	   if (item.IsFirstClosing)
		   row.cell.Add(Html.Image("Tick.gif").ToHtmlString());
	   else
		   row.cell.Add(string.Empty);
	   row.cell.Add(Html.Image("Edit.gif", new { @style = "cursor:pointer", @onclick = "javascript:fundClosing.add(" + item.FundClosingID.ToString() + ");" }).ToHtmlString() + "&nbsp;&nbsp;&nbsp;" +
				  Html.Image("Delete.png", new { @style = "cursor:pointer", @onclick = "javascript:fundClosing.deleteFundClosing(" + item.FundClosingID.ToString() + ",this);" }).ToHtmlString());
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>
