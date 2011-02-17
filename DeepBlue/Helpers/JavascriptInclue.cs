using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeepBlue.Helpers {
	public static class JavascriptInclue {
		public static string JavascriptInclueTag(this HtmlHelper helper, string scriptname) {
			return string.Format("<script src=\"/Assets/javascripts/{0}\" type=\"text/javascript\"></script>", scriptname);
		}
	}
}