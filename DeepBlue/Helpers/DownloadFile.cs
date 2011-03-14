using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace DeepBlue.Helpers {
	public class DownloadFile : ActionResult {
		public DownloadFile() {
		}

		public DownloadFile(string virtualPath) {
			this.VirtualPath = virtualPath;
		}

		public string VirtualPath { get; set; }

		public string FileDownloadName { get; set; }

		public override void ExecuteResult(ControllerContext context) {
			if (string.IsNullOrEmpty(FileDownloadName) == false) {
				context.HttpContext.Response.AddHeader("content-disposition", "attachment;filename=" + this.FileDownloadName);
			}
			context.HttpContext.Response.WriteFile(Path.Combine(context.HttpContext.Server.MapPath("/"), this.VirtualPath));
		}
	}

}