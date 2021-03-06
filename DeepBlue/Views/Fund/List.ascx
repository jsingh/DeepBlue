﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<DeepBlue.Models.Fund.FundListModel>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% FlexigridData flexData = new FlexigridData();%>
<% flexData.total = Convert.ToInt32(ViewData["TotalRows"]);
   flexData.page = Convert.ToInt32(ViewData["PageNo"]);
   FlexigridRow row;
   foreach (var item in Model) {
	   row = new FlexigridRow();
	   row.cell.Add(item.FundId);
	   row.cell.Add(item.FundName);
	   row.cell.Add(item.TaxId);
	   row.cell.Add((item.InceptionDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy"));
	   row.cell.Add((item.ScheduleTerminationDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy"));
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>