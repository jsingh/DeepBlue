using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using Winnovative.WnvHtmlConvert;
using System.Web.Mvc;


namespace DeepBlue.Helpers {
	public class ExportToPdf : ActionResult {

		private string _FileName;
		private string _Html;

		public ExportToPdf(string fileName, string html) {
			_FileName = fileName;
			_Html = html;
		}

		public override void ExecuteResult(ControllerContext context) {
			string rootPath = HttpContext.Current.Server.MapPath("~/");
			string exportPdfPath = Path.Combine(rootPath, ConfigurationManager.AppSettings["ExportPdfPath"]);
			PdfConverter pdfConverterObj = new PdfConverter();
			string sourcePath = Path.Combine(exportPdfPath, _FileName.Replace(".pdf", ".htm"));
			string destinationPath = Path.Combine(exportPdfPath, _FileName);

			if (File.Exists(sourcePath)) {
				File.Delete(sourcePath);
			}
			if (File.Exists(destinationPath)) {
				File.Delete(destinationPath);
			}

			byte[] pdfBytes;
			pdfConverterObj.LicenseKey = "saFXUumYqQkFyGSJMqYqJTEaCLsFY+YJvYZ1w/jOpg/SALDg50HlcMtPcu1Xbinw";
			pdfConverterObj.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;

			pdfConverterObj.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
			pdfConverterObj.PdfDocumentOptions.ShowHeader = false;
			pdfConverterObj.PdfDocumentOptions.ShowFooter = false;
			pdfConverterObj.PdfDocumentOptions.LeftMargin = 40;
			pdfConverterObj.PdfDocumentOptions.RightMargin = 40;
			pdfConverterObj.PdfDocumentOptions.TopMargin = 40;
			pdfConverterObj.PdfDocumentOptions.BottomMargin = 40;
			pdfConverterObj.PdfDocumentOptions.GenerateSelectablePdf = true;
			pdfConverterObj.PdfDocumentOptions.LiveUrlsEnabled = true;
			pdfConverterObj.PdfBookmarkOptions.TagNames = null;

			File.WriteAllText(sourcePath, _Html);

			if (File.Exists(sourcePath)) {

				pdfBytes = pdfConverterObj.GetPdfBytesFromHtmlFile(sourcePath);

				if (File.Exists(destinationPath)) {
					File.Delete(destinationPath);
				}
				File.WriteAllBytes(destinationPath, pdfBytes);
				if (File.Exists(destinationPath)) {
					context.HttpContext.Response.Buffer = true;
					context.HttpContext.Response.Clear();
					context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + _FileName);
					context.HttpContext.Response.ContentType = "application/pdf";
					context.HttpContext.Response.WriteFile(destinationPath);
				}
			}
		}
	}
}