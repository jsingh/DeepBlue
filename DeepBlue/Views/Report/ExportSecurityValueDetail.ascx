<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Report.ExportSecurityValueDetailModel>" %>
<%if (Model.SecurityValueReportDetails.Count > 0) {%>
<table cellpadding="0" cellspacing="10" border="0" id="SecurityValueList">
	<thead>
		<tr>
			<th>
				Deal
			</th>
			<th>
				Security
			</th>
			<%--	<th>
				Security Type
			</th>--%>
			<th>
				#Shares
			</th>
			<th>
				Price
			</th>
			<th>
				Price Date
			</th>
			<th>
				Total Value
			</th>
		</tr>
	</thead>
	<tbody>
		<%foreach (DeepBlue.Models.Report.SecurityValueReportDetail distribution in Model.SecurityValueReportDetails) {%>
		<tr>
			<td>
				<%=distribution.DealNo%>
			</td>
			<td>
				<%=distribution.Security%>
			</td>
			<%--	<td>
				<%=distribution.SecurityType%>
			</td>--%>
			<td>
				<%=distribution.NoOfShares%>
			</td>
			<td>
				<%=distribution.Price%>
			</td>
			<td>
				<%=(distribution.Date ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy")%>
			</td>
			<td>
				<%=distribution.Value%>
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