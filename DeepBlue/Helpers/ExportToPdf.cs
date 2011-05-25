using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;


namespace DeepBlue.Helpers {
	public class ExportToPdf {
		public static string Export(string fileName, string html) {
			string rootPath = HttpContext.Current.Server.MapPath("/");
			string exportPdfPath = Path.Combine(rootPath, ConfigurationManager.AppSettings["ExportPdfPath"]);
			return string.Empty;
		}
	}
}