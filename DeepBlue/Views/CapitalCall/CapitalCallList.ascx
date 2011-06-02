<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<DeepBlue.Models.Entity.CapitalCall>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% FlexigridData flexData = new FlexigridData();%>
<% flexData.total = Convert.ToInt32(ViewData["TotalRows"]);
   flexData.page = Convert.ToInt32(ViewData["PageNo"]);
   FlexigridRow row;
   foreach (var item in Model) {
	   row = new FlexigridRow();
	   row.cell.Add(item.CapitalCallID.ToString());
	   row.cell.Add(item.CapitalCallNumber.ToString());
	   row.cell.Add(FormatHelper.CurrencyFormat(item.CapitalAmountCalled));
	   row.cell.Add(FormatHelper.CurrencyFormat(item.ManagementFees));
	   row.cell.Add(FormatHelper.CurrencyFormat(item.FundExpenses));
	   row.cell.Add(item.CapitalCallDate.ToString("MM/dd/yyyy"));
	   row.cell.Add(item.CapitalCallDueDate.ToString("MM/dd/yyyy"));
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>
