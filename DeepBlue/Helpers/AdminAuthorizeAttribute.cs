using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DeepBlue.Helpers {
	public class AdminAuthorizeAttribute : AuthorizeAttribute {
		protected override bool AuthorizeCore(HttpContextBase httpContext) {
			return AdminAuthorizeHelper.IsAdmin;
		}
	}
}
