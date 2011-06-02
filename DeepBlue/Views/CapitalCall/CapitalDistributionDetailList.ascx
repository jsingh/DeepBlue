<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<DeepBlue.Models.Entity.CapitalDistribution>>" %>
<%@ Import Namespace="DeepBlue.Helpers" %>
<% FlexigridData flexData = new FlexigridData();%>
<% flexData.total = Convert.ToInt32(ViewData["TotalRows"]);
   flexData.page = Convert.ToInt32(ViewData["PageNo"]);
   FlexigridRow row;
   foreach (var item in Model) {
	   row = new FlexigridRow();
	   row.cell.Add(item.CapitalDistributionID.ToString());
	   row.cell.Add(item.DistributionNumber.ToString());
	   row.cell.Add(FormatHelper.CurrencyFormat(item.DistributionAmount));
	   row.cell.Add(FormatHelper.CurrencyFormat(item.ReturnManagementFees));
	   row.cell.Add(FormatHelper.CurrencyFormat(item.ReturnFundExpenses));
	   row.cell.Add(item.CapitalDistributionDate.ToString("MM/dd/yyyy"));
	   row.cell.Add(item.CapitalDistributionDueDate.ToString("MM/dd/yyyy"));
	   flexData.rows.Add(row);
   } %>
<%= JsonSerializer.ToJsonObject(flexData)%>