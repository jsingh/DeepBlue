<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Report.ExportUnfundedCapitalCallBalanceDetailModel>" %>
<%if (Model.UnfundedCapitalCallBalanceReportDetails.Count > 0) {%>
<table cellpadding="0" cellspacing="10" border="0" id="UnfundedCapitalCallBalanceList">
	<thead>
		<tr>
			<th>
				Deal No
			</th>
			<th>
				Fund
			</th>
			<th>
				Unfunded
			</th>
		</tr>
	</thead>
	<tbody>
		<%foreach (DeepBlue.Models.Report.UnfundedCapitalCallBalanceReportDetail distribution in Model.UnfundedCapitalCallBalanceReportDetails) {%>
		<tr>
			<td>
				<%=distribution.DealNo%>
			</td>
			<td>
				<%=distribution.FundName%>
			</td>
			<td>
				<%=distribution.UnfundedAmount%>
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