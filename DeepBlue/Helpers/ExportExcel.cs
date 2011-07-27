using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.IO;
using System.Web.UI;
using System.Web.Mvc;
using System.Configuration;


namespace DeepBlue.Helpers {
	public class ExportExcel {

		public ExportExcel() {
			_FileName = string.Empty;
		}

		private string _FileName;

		public object Data { get; set; }

		public string TableName { get; set; }

		public string FileName {
			get {
				return _FileName.Replace("\\","/");
			}
		}

		public bool Export() {
			_FileName = Path.Combine(string.Format(ConfigurationManager.AppSettings["ExportExcelPath"], TableName + ".xls"));
			string rootPath = HttpContext.Current.Server.MapPath("/");
			string exportExcelFilePath = Path.Combine(rootPath,_FileName);
			string directoryName = Path.GetDirectoryName(exportExcelFilePath);
			string result = string.Empty;
			
			System.Web.UI.WebControls.GridView grd = new System.Web.UI.WebControls.GridView();
			grd.DataSource = Data;
			grd.DataBind();
			
			if (Directory.Exists(directoryName) == false) {
				Directory.CreateDirectory(directoryName);
			}
			
			StringWriter swr = new StringWriter();
			HtmlTextWriter tw = new HtmlTextWriter(swr);
			
			grd.RenderControl(tw);
			
			if (File.Exists(exportExcelFilePath)) {
				File.Delete(exportExcelFilePath);
			}
			
			File.WriteAllText(exportExcelFilePath, swr.ToString());

			if (File.Exists(exportExcelFilePath)) {
				return true;
			}else{
				return false;
			}
		}

	}
}