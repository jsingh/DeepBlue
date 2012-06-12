using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeepBlue.Helpers {
	public class AuthorizeUsersAttribute : AuthorizeAttribute {
		protected override bool AuthorizeCore(HttpContextBase httpContext) {
			return Authentication.CurrentUser != null;
		}
	}
}