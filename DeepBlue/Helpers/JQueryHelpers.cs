﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace DeepBlue.Helpers {
	public static class JQueryHelpers {
		public static string jQueryAutoCompleteScript(this HtmlHelper helper, string targetId, AutoCompleteOptions options) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targetId + "\").autocomplete({")
					 .Append("source:\"" + options.Source + "\"")
					 .Append(",minLength:" + options.MinLength.ToString())
					 .Append((string.IsNullOrEmpty(options.OnSelect) ? "" : ",select:" + options.OnSelect.ToString()))
					 .Append((string.IsNullOrEmpty(options.OnChange)? "" : ",change:" + options.OnChange.ToString()))
					 .Append(",appendTo:\"" + options.AppendTo + "\"")
					 .Append(",delay:" + options.Delay.ToString());
			scriptSrc.Append("});});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}

		public static string jQueryAccordionScript(this HtmlHelper helper, string targetId, AccordionOptions options) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targetId + "\").accordion({")
					 .Append("active:" + options.Active.ToString() + "")
					 .Append(",autoHeight: " + options.AutoHeight.ToString().ToLower())
					 .Append(",disabled: " + options.Disabled.ToString().ToLower())
					 .Append(",collapsible: " + options.Collapsible.ToString().ToLower());
			scriptSrc.Append("});});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}

		public static string jQueryDatePickerScript(this HtmlHelper helper, string targetId) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targetId + "\").datepicker({changeMonth: true, changeYear: true});});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}

		public static string jQueryTab(this HtmlHelper helper, string targetId, jQueryTabOptions options) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targetId + "\").tabs({")
							 .Append("collapsible:" + options.Collapsible.ToString().ToLower() + "")
					 .Append("});});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}

		public static string jQueryFlexiGrid(this HtmlHelper helper, string targetId) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targetId + "\").flexigrid();});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}
	}
}