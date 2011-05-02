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
	public class ExportWordResult : ActionResult {

		public string Url { get; set; }

		public string FileName { get; set; }

		public ExportWordResult(string url, string fileName)
        {
            Url = url;
            FileName = fileName;
        }


        public override void ExecuteResult(ControllerContext context)
        {
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
        }

	}
}