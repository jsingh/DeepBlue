using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeepBlue.Helpers {
	public static class CssInclude {
		public static string CssIncludeTag(this HtmlHelper helper, string cssname) {
			return string.Format("<link href=\"/Assets/stylesheets/{0}\" rel=\"stylesheet\" type=\"text/css\" />", cssname);
		}
	}
}