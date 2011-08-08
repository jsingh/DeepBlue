using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.IO;
using System.Web.UI;
using System.Web.Mvc;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Web.UI.WebControls;


namespace DeepBlue.Helpers {
	public class ExportExcel : ActionResult {

		public ExportExcel() {
		}

		public string TableName { get; set; }

		public object GridData { get; set; }

		public override void ExecuteResult(ControllerContext context) {
			DataGrid grd = new DataGrid();

			grd.DataSource = GridData;
			grd.DataBind();

			context.HttpContext.Response.ClearContent();
			context.HttpContext.Response.AddHeader("content-disposition", "attachment;filename=" + TableName + ".xls");
			context.HttpContext.Response.ContentType = "application/excel";

			StringWriter swr = new StringWriter();
			HtmlTextWriter tw = new HtmlTextWriter(swr);
			grd.RenderControl(tw);
			context.HttpContext.Response.Write(swr.ToString());
			context.HttpContext.Response.End();
		}

	 
	}
}