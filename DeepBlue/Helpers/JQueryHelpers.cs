using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace DeepBlue.Helpers {
	public static class JQueryHelpers {
		public static string jQueryAutoComplete(this HtmlHelper helper, string targetId, AutoCompleteOptions options) {
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

		public static string jQueryAccordion(this HtmlHelper helper, string targetId, AccordionOptions options) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targetId + "\").accordion({")
					 .Append("active:" + options.Active.ToString() + "")
					 .Append(",autoHeight: " + options.AutoHeight.ToString().ToLower())
					 .Append(",disabled: " + options.Disabled.ToString().ToLower())
					 .Append(",collapsible: " + options.Collapsible.ToString().ToLower());
			scriptSrc.Append("});});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}

		public static string jQueryDatePicker(this HtmlHelper helper, string targetId) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targetId + "\").datepicker({changeMonth: true, changeYear: true")
					 .Append("});});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}

		public static string jQueryDatePicker(this HtmlHelper helper, string targetId, DatePickerOptions options) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targetId + "\").datepicker({changeMonth: true, changeYear: true")
					 .Append((string.IsNullOrEmpty(options.OnBeforeShow)==false ? ",beforeShow:" + options.OnBeforeShow + "" : ""))
					 .Append((string.IsNullOrEmpty(options.OnClose)==false ? ",onClose:" + options.OnClose + "" : ""))
					 .Append((string.IsNullOrEmpty(options.OnSelect)==false ? ",onSelect:" + options.OnSelect + "" : ""))
					 .Append("});});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}
	 
		public static string jQueryFlexiGrid(this HtmlHelper helper, string targetId, FlexigridOptions options) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targetId + "\").flexigrid({")
					 .Append("usepager:"+ options.Paging.ToString().ToLower())
					 .Append(",url:\"/" + options.ControllerName.ToString() + "/" + options.ActionName.ToString() + "\"")
					 .Append(",method:\"" + options.HttpMethod.ToString() + "\"")
					 .Append(",sortname:\"" + options.SortName.ToString() + "\"")
					 .Append(",sortorder:\"" + options.SortOrder.ToString() + "\"")
					 .Append(",autoload:" + options.Autoload.ToString().ToLower() + "")
					 .Append("});});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}
	}
}