using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using Winnovative.WnvHtmlConvert;
using System.Web.Mvc;


namespace DeepBlue.Helpers {
	public class ExportToPdf:ActionResult {

		private string _FileName;
		private string _Html;

		public ExportToPdf(string fileName,string html) {
			_FileName=fileName;
			_Html=html;
		}

		public override void ExecuteResult(ControllerContext context) {
			PdfConverter pdfConverterObj=new PdfConverter();

			Guid guid=System.Guid.NewGuid();
			string fileName=guid.ToString()+".pdf";

			string sourceFileName=fileName.Replace(".pdf",".htm");
			string destinationFileName=fileName;

			if(UploadFileHelper.TempFileExist(sourceFileName)) {
				UploadFileHelper.TempFileDelete(sourceFileName);
			}

			if(UploadFileHelper.TempFileExist(destinationFileName)) {
				UploadFileHelper.TempFileDelete(destinationFileName);
			}

			byte[] pdfBytes;
			pdfConverterObj.LicenseKey="saFXUumYqQkFyGSJMqYqJTEaCLsFY+YJvYZ1w/jOpg/SALDg50HlcMtPcu1Xbinw";
			pdfConverterObj.PdfDocumentOptions.PdfPageSize=PdfPageSize.A4;

			pdfConverterObj.PdfDocumentOptions.PdfCompressionLevel=PdfCompressionLevel.Normal;
			pdfConverterObj.PdfDocumentOptions.ShowHeader=false;
			pdfConverterObj.PdfDocumentOptions.ShowFooter=false;
			pdfConverterObj.PdfDocumentOptions.LeftMargin=40;
			pdfConverterObj.PdfDocumentOptions.RightMargin=40;
			pdfConverterObj.PdfDocumentOptions.TopMargin=40;
			pdfConverterObj.PdfDocumentOptions.BottomMargin=40;
			pdfConverterObj.PdfDocumentOptions.GenerateSelectablePdf=true;
			pdfConverterObj.PdfDocumentOptions.LiveUrlsEnabled=true;
			pdfConverterObj.PdfBookmarkOptions.TagNames=null;

			FileInfo htmlFileInfo=UploadFileHelper.WriteTempFileText(sourceFileName,_Html);

			if(UploadFileHelper.TempFileExist(sourceFileName)) {

				pdfBytes=pdfConverterObj.GetPdfBytesFromHtmlFile(htmlFileInfo.FullName);

				FileInfo destFileInfo=UploadFileHelper.TempFileWriteAllBytes(destinationFileName,pdfBytes);
				
				UploadFileHelper.TempFileDelete(htmlFileInfo.Name);

				if(UploadFileHelper.TempFileExist(destFileInfo.Name)) {
					byte[] bytes=System.IO.File.ReadAllBytes(destFileInfo.FullName);
					UploadFileHelper.TempFileDelete(destFileInfo.Name);
					context.HttpContext.Response.Buffer=true;
					context.HttpContext.Response.Clear();
					context.HttpContext.Response.AddHeader("content-disposition","attachment; filename="+fileName);
					context.HttpContext.Response.ContentType="application/pdf";
					context.HttpContext.Response.BinaryWrite(bytes);
				}
			}
		}
	}
}