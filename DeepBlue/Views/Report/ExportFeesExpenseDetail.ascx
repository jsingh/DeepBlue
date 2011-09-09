<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Report.ExportFeeExpenseDetailModel>" %>
<%if (Model.FeesExpenseReportDetails.Count > 0) {%>
<table cellpadding="0" cellspacing="10" border="0" id="FeesExpenseList">
	<thead>
		<tr>
			<th>
				Date
			</th>
			<th>
				Type
			</th>
			<th>
				Amount
			</th>
			<th>
				Note
			</th>
		</tr>
	</thead>
	<tbody>
		<%foreach (DeepBlue.Models.Report.FeesExpenseReportDetail fee in Model.FeesExpenseReportDetails) {%>
		<tr>
			<td>
				<%=(fee.Date ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy")%>
			</td>
			<td>
				<%=fee.Type%>
			</td>
			<td>
				<%=fee.Amount%>
			</td>
			<td>
				<%=fee.Note%>
			</td>
		</tr>
		<%}%>
	</tbody>
</table>
<%}%>
<%DeepBlue.Models.Admin.Enums.ExportType exportType = (DeepBlue.Models.Admin.Enums.ExportType)Model.ExportTypeId;
  if (exportType == DeepBlue.Models.Admin.Enums.ExportType.Excel || exportType == DeepBlue.Models.Admin.Enums.ExportType.Word) {
	  bool exportReady = false;
	  switch (exportType) {
		  case DeepBlue.Models.Admin.Enums.ExportType.Word:
			  Response.AddHeader("content-disposition", "attachment;filename=DealReport.doc");
			  Response.ContentType = "application/ms-word";
			  exportReady = true;
			  break;
		  case DeepBlue.Models.Admin.Enums.ExportType.Excel:
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