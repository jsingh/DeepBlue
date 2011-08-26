using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {

	public static class AdminAuthorizeHelper {
		public static bool IsAdmin {
			get {
				try {
					return Authentication.CurrentUser.IsAdmin;
				}
				catch (AuthenticationException) {
					return false;
				}
			}
		}
	}
}