using System;
using System.Web;
using System.Web.Routing;

namespace DeepBlue.Helpers {
	public class FileUploadRouteHandler : IRouteHandler {

		public IHttpHandler GetHttpHandler(RequestContext requestContext) {
			return new FileUploadHandler(requestContext);
		}
	}
}
