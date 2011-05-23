using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;

namespace DeepBlue.Helpers {
	public class ExportPdfResult : ActionResult {
		public string Url { get; set; }

		public string FileName { get; set; }

		public ExportPdfResult(string url, string fileName) {
			Url = url;
			FileName = fileName;
		}

		public override void ExecuteResult(ControllerContext context) {
			HttpWebRequest wreq = (HttpWebRequest)HttpWebRequest.Create(this.Url);
			HttpWebResponse wres = (HttpWebResponse)wreq.GetResponse();
			Stream s = wres.GetResponseStream();
			StreamReader sr = new StreamReader(s, Encoding.ASCII);
			string exportPdfFileName = ExportToPdf.Export(FileName, sr.ReadToEnd());
			if (File.Exists(exportPdfFileName)) {
				context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
				context.HttpContext.Response.ContentType = "application/octet-stream";
				context.HttpContext.Response.BinaryWrite(File.ReadAllBytes(exportPdfFileName));
				context.HttpContext.Response.End();
			}
		}
	}
}