<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Report.ExportUnderlyingFundNAVDetailModel>" %>
<%if (Model.UnderlyingFundNAVReportDetails.Count > 0) {%>
<table cellpadding="0" cellspacing="10" border="0" id="UnderlyingFundNAVList">
	<thead>
		<tr>
			<th>
				Date
			</th>
			<th>
				Deal
			</th>
			<th>
				Fund
			</th>
			<th>
				NAV
			</th>
			<th>
				Receipt
			</th>
			<th>
				Frequency
			</th>
			<th>
				Method
			</th>
		</tr>
	</thead>
	<tbody>
		<%foreach (DeepBlue.Models.Report.UnderlyingFundNAVReportDetail distribution in Model.UnderlyingFundNAVReportDetails) {%>
		<tr>
			<td>
				<%=(distribution.Date ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy")%>
			</td>
			<td>
				<%=distribution.DealNo%>
			</td>
			<td>
				<%=distribution.FundName%>
			</td>
			<td>
				<%=distribution.NAV%>
			</td>
			<td>
				<%=(distribution.Receipt ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy")%>
			</td>
			<td>
				<%=distribution.Frequency%>
			</td>
			<td>
				<%=distribution.Method%>
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