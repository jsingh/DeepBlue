using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using System.Web.Security;
using DeepBlue.Controllers.Account;


namespace DeepBlue.Helpers {
	public static class Authentication {

		private const string entityKey = "CurrentEntity";
		private const string user = "CurrentUser";

		public static bool IsSystemEntityUser {
			get {
				if (Authentication.CurrentEntity != null)
					return (Authentication.CurrentEntity.EntityID == (int)ConfigUtil.SystemEntityID);
				else
					return false;
			}
		}

		public static USER CurrentUser {
			get {
				if (SecurityContainer.GetHttpContext().Session[user] != null)
					return (USER)SecurityContainer.GetHttpContext().Session[user];
				else
					return null;
			}
			set {
				SecurityContainer.GetHttpContext().Session[user] = value;
			}
		}

		public static ENTITY CurrentEntity {
			get {
				if (SecurityContainer.GetHttpContext().Session[entityKey] != null)
					return (ENTITY)SecurityContainer.GetHttpContext().Session[entityKey];
				else
					return null;
			}
			set {
				SecurityContainer.GetHttpContext().Session[entityKey] = value;
			}
		}

		public static void Flush() {
			Authentication.CurrentUser = null;
			Authentication.CurrentEntity = null;
		}

	}


}