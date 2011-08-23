using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace DeepBlue.Helpers {
	public static class AppSettingsHelper {
		public static int CurrentEntityID {
			get {
				return	Convert.ToInt32(ConfigurationManager.AppSettings["CurrentEntityID"]);
			}
		}
	}
}