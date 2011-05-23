using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using Winnovative.WnvHtmlConvert;

namespace DeepBlue.Helpers {
	public class ExportToPdf {

		public static string Export(string fileName, string html) {
			string rootPath = HttpContext.Current.Server.MapPath("/");
			string exportPdfPath = Path.Combine(rootPath, ConfigurationManager.AppSettings["ExportPdfPath"]);
			PdfConverter pdfConvert = null;
			if (Directory.Exists(exportPdfPath) == false) {
				Directory.CreateDirectory(exportPdfPath);
			}
			string htmlFileName = Path.Combine(exportPdfPath, fileName.Replace(".pdf", ".html"));
			if (File.Exists(htmlFileName)) {
				File.Delete(htmlFileName);
			}
			File.WriteAllText(htmlFileName, html);
			string pdfFileName = string.Empty;
			if (File.Exists(htmlFileName)) {
				pdfFileName = Path.Combine(exportPdfPath, fileName);
				pdfConvert = new PdfConverter();
				pdfConvert.LicenseKey = "saFXUumYqQkFyGSJMqYqJTEaCLsFY+YJvYZ1w/jOpg/SALDg50HlcMtPcu1Xbinw";

				pdfConvert.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
				pdfConvert.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
				pdfConvert.PdfDocumentOptions.ShowHeader = false;
				pdfConvert.PdfDocumentOptions.ShowFooter = false;
				pdfConvert.PdfDocumentOptions.LeftMargin = 10;
				pdfConvert.PdfDocumentOptions.RightMargin = 10;
				pdfConvert.PdfDocumentOptions.TopMargin = 10;
				pdfConvert.PdfDocumentOptions.BottomMargin = 10;
				pdfConvert.PdfDocumentOptions.GenerateSelectablePdf = true;
				pdfConvert.PdfDocumentOptions.LiveUrlsEnabled = true;
				pdfConvert.PdfBookmarkOptions.TagNames = null;

				Byte[] pdfBytes = pdfConvert.GetPdfFromUrlBytes(htmlFileName);

				if (File.Exists(pdfFileName)) {
					File.Delete(pdfFileName);
				}

				File.WriteAllBytes(pdfFileName, pdfBytes);

				//if (File.Exists(htmlFileName)) {
				//    File.Delete(htmlFileName);
				//}
				
			}
			return pdfFileName;
		}
	}
}