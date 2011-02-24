using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace DeepBlue.Helpers {
	public static class JQueryHelpers {
		public static string jQueryAutoCompleteScript(this HtmlHelper helper, string targerId, AutoCompleteOptions options) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targerId + "\").autocomplete({")
					 .Append("source:\"" + options.Source + "\"")
					 .Append(",minLength:" + options.MinLength.ToString())
					 .Append((string.IsNullOrEmpty(options.OnSelect) ? "" : ",select:" + options.OnSelect.ToString()))
					 .Append((string.IsNullOrEmpty(options.OnChange)? "" : ",change:" + options.OnChange.ToString()))
					 .Append(",appendTo:\"" + options.AppendTo + "\"")
					 .Append(",delay:" + options.Delay.ToString());
			scriptSrc.Append("});});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}

		public static string jQueryAccordionScript(this HtmlHelper helper, string targerId, AccordionOptions options) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targerId + "\").accordion({")
					 .Append("active:" + options.Active.ToString() + "")
					 .Append(",autoHeight: " + options.AutoHeight.ToString().ToLower())
					 .Append(",collapsible: " + options.Collapsible.ToString().ToLower());
			scriptSrc.Append("});});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}

		public static string jQueryDatePickerScript(this HtmlHelper helper, string targerId) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targerId + "\").datepicker();});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}
	}
}