<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DeepBlue.Models.Report.ExportCashDistributionDetailModel>" %>
<%Html.RenderPartial("CashDistributionSummaryReport", Model.CashDistributionReportDetail);%>
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