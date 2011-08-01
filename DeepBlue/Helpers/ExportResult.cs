using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Net;
using System.IO;
using System.Text;

namespace DeepBlue.Helpers {
	public class ExportResult : ActionResult {

		public string Url { get; set; }

		public string FileName { get; set; }

		public object Data { get; set; }

		public Models.Deal.Enums.ExportType ExportType { get; set; }

		public ExportResult(object data, Models.Deal.Enums.ExportType exportType) {
			Data = data;
			ExportType = exportType;
		}

		public override void ExecuteResult(ControllerContext context) {

			HttpContext curContext = HttpContext.Current;
			curContext.Response.Clear();
			curContext.Response.AddHeader("content-disposition", "attachment;filename=" + this.FileName);
			curContext.Response.Charset = "";
			curContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
			curContext.Response.ContentType = "application/ms-word";

			HttpWebRequest wreq = (HttpWebRequest)HttpWebRequest.Create(this.Url);
			HttpWebResponse wres = (HttpWebResponse)wreq.GetResponse();
			Stream s = wres.GetResponseStream();
			StreamReader sr = new StreamReader(s, Encoding.ASCII);

			curContext.Response.Write(sr.ReadToEnd());
			curContext.Response.End();

			//Bind the Data to Asp.Net DataGrid - you can still use this in Asp.Net MVC though it 
			//cannot be used in the .aspx View
			System.Web.UI.WebControls.GridView grd = new System.Web.UI.WebControls.GridView();
			grd.DataSource = this.Data;
			grd.DataBind();
			HttpContext.Current.Response.ClearContent();
			bool exportReady = false;
			switch (ExportType) {
				case Models.Deal.Enums.ExportType.Word:
					HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=DealReport.doc");
					HttpContext.Current.Response.ContentType = "application/ms-word";
					exportReady = true;
					break;
				case Models.Deal.Enums.ExportType.Excel:
					HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=DealReport.xls");
					HttpContext.Current.Response.ContentType = "application/excel";
					exportReady = true;
					break;
			}
			if (exportReady) {
				StringWriter swr = new StringWriter();
				HtmlTextWriter tw = new HtmlTextWriter(swr);
				grd.RenderControl(tw);
				HttpContext.Current.Response.Write(swr.ToString());
				HttpContext.Current.Response.End();
			}
		}

	}
}