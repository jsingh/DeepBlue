﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IList<DeepBlue.Models.Entity.InvestorType>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% FlexigridData flexData = new FlexigridData();%>
<% flexData.total = Convert.ToInt32(ViewData["TotalRows"]);
   flexData.page = Convert.ToInt32(ViewData["PageNo"]);
   FlexigridRow row;
   foreach (var item in Model) {
	   row = new FlexigridRow();
	   row.cell.Add(item.InvestorTypeID.ToString());
	   row.cell.Add(item.InvestorTypeName);
	   row.cell.Add(item.Enabled);
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>
