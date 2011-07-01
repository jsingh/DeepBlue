using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace DeepBlue.Helpers {
	public class FileUploadHandler : IHttpHandler {
		/// <summary>
		/// You will need to configure this handler in the web.config file of your 
		/// web and register it with IIS before being able to use it. For more information
		/// see the following link: http://go.microsoft.com/?linkid=8101007
		/// </summary>
		/// 

		public FileUploadHandler(RequestContext context) {
			ProcessRequest(context);
		}

		#region IHttpHandler Members

		public bool IsReusable {
			// Return false in case your Managed Handler cannot be reused for another request.
			// Usually this would be false in case you have some state information preserved per request.
			get { return true; }
		}

		public void ProcessRequest(RequestContext requestContext) {
			//write your handler implementation here.
		}

		#endregion


		public void ProcessRequest(HttpContext context) {
			throw new NotImplementedException();
		}
	}
}