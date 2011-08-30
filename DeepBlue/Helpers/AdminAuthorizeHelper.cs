using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {

	public static class AdminAuthorizeHelper {
		public static bool IsAdmin {  
			get {          
				if(Authentication.CurrentUser != null)
					return Authentication.CurrentUser.IsAdmin;
				else
					return false;
			}
		}
	}
}