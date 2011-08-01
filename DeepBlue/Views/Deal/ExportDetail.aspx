<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DeepBlue.Models.Deal.DealExportModel>" %>

<%@ Import Namespace="DeepBlue.Helpers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Export</title>
	<style type="text/css">
		body {
			margin: 0;
			padding: 0;
			font-size: 12px;
			font-family: Arial;
		}
		th, td {
			text-align: left;
			padding: 5px;
			font-weight: normal;
		}
	</style>
</head>
<body>
	<table cellpadding="0" cellspacing="1" border="0" id="ReportList">
		<thead>
			<tr class="report_tr">
				<th style="width: 5%">
					<span>Deal No.</span>
				</th>
				<th style="width: 18%">
					<span>Deal Name</span>
				</th>
				<th style="width: 12%">
					<span>Deal Date</span>
				</th>
				<th style="text-align: right; width: 12%">
					<span>Net Purchase Price</span>
				</th>
				<th style="text-align: right; width: 12%">
					<span>Gross Purchase Price</span>
				</th>
				<th style="text-align: right; width: 12%">
					<span>Commitment Amount</span>
				</th>
				<th style="text-align: right; width: 12%">
					<span>Unfunded Amount</span>
				</th>
				<th style="text-align: right; width: 12%">
					<span>Total Amount</span>
				</th>
			</tr>
		</thead>
		<tbody>
			<% foreach (var item in Model.Deals) { %>
			<tr>
				<td>
					<%: item.DealNumber %>
				</td>
				<td>
					<%: item.DealName %>
				</td>
				<td>
					<%: item.DealDate.ToString("MM/dd/yyyy") %>
				</td>
				<td style="text-align: right">
					<%: FormatHelper.CurrencyFormat(item.NetPurchasePrice)%>
				</td>
				<td style="text-align: right">
					<%: FormatHelper.CurrencyFormat(item.GrossPurchasePrice)%>
				</td>
				<td style="text-align: right">
					<%: FormatHelper.CurrencyFormat(item.CommittedAmount)%>
				</td>
				<td style="text-align: right">
					<%: FormatHelper.CurrencyFormat(item.UnfundedAmount)%>
				</td>
				<td style="text-align: right">
					<%: FormatHelper.CurrencyFormat(item.TotalAmount)%>
				</td>
			</tr>
			<% } %>
		</tbody>
	</table>
	<%DeepBlue.Models.Deal.Enums.ExportType exportType = (DeepBlue.Models.Deal.Enums.ExportType)ViewData["ExportTypeId"];
   if (exportType == DeepBlue.Models.Deal.Enums.ExportType.Excel || exportType == DeepBlue.Models.Deal.Enums.ExportType.Word) {
	   bool exportReady = false;
	   switch (exportType) {
		   case DeepBlue.Models.Deal.Enums.ExportType.Word:
			   Response.AddHeader("content-disposition", "attachment;filename=DealReport.doc");
			   Response.ContentType = "application/ms-word";
			   exportReady = true;
			   break;
		   case DeepBlue.Models.Deal.Enums.ExportType.Excel:
			   Response.AddHeader("content-disposition", "attachment;filename=DealReport.xls");
			   Response.ContentType = "application/excel";
			   exportReady = true;
			   break;
	   }
	   if (exportReady) {
		   System.IO.StringWriter swr = new System.IO.StringWriter();
		   HtmlTextWriter tw = new HtmlTextWriter(swr);
		   Response.Write(swr.ToString());
		   Response.End();
	   }
   }
	%>
</body>
</html>
