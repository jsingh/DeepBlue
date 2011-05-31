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
					 .Append((string.IsNullOrEmpty(options.OnChange) ? "" : ",change:" + options.OnChange.ToString()))
					 .Append((string.IsNullOrEmpty(options.OnSearch) ? "" : ",search:" + options.OnSearch.ToString()))
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

		public static string jQueryNumericTextBox(this HtmlHelper helper, string targetId) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targetId + "\").keyPress(function(event){jHelper.isNumeric(this);})});");
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
					 .Append((string.IsNullOrEmpty(options.OnBeforeShow) == false ? ",beforeShow:" + options.OnBeforeShow + "" : ""))
					 .Append((string.IsNullOrEmpty(options.OnBeforeShowDay) == false ? ",beforeShowDay:" + options.OnBeforeShowDay + "" : ""))
					 .Append((string.IsNullOrEmpty(options.OnClose) == false ? ",onClose:" + options.OnClose + "" : ""))
					 .Append((string.IsNullOrEmpty(options.OnSelect) == false ? ",onSelect:" + options.OnSelect + "" : ""))
					 .Append((string.IsNullOrEmpty(options.OnCreate) == false ? ",create:" + options.OnCreate + "" : ""))
					 .Append((string.IsNullOrEmpty(options.OnChangeMonthYear) == false ? ",onChangeMonthYear:" + options.OnChangeMonthYear + "" : ""))
					 .Append("});});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}

		public static string jQueryFlexiGrid(this HtmlHelper helper, string targetId, FlexigridOptions options) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targetId + "\").flexigrid({")
					 .Append("usepager:" + options.Paging.ToString().ToLower());
			if (string.IsNullOrEmpty(options.ControllerName) == false && string.IsNullOrEmpty(options.ActionName) == false) {
				scriptSrc.Append(",url:\"/" + options.ControllerName.ToString() + "/" + options.ActionName.ToString() + "\"");
			}
			if (string.IsNullOrEmpty(options.OnSuccess) == false) {
				scriptSrc.Append(",onSuccess:" + options.OnSuccess + "");
			}
			if (string.IsNullOrEmpty(options.OnSubmit) == false) {
				scriptSrc.Append(",onSubmit:" + options.OnSubmit + "");
			}
			if(string.IsNullOrEmpty(options.OnRowClick)==false){
				scriptSrc.Append(",onRowClick:" + options.OnRowClick + "");
			}
			if (string.IsNullOrEmpty(options.OnRowBound) == false) {
				scriptSrc.Append(",onRowBound:" + options.OnRowBound + "");
			}
			scriptSrc.Append(",rpOptions:[");
			string rows = string.Empty;
			foreach (var value in options.RowOptions) {
				rows +=  value + ",";
			}
			if (rows.Length > 0) {
				rows = rows.Substring(0, rows.Length - 1);
			}
			scriptSrc.Append(rows + "]");
			scriptSrc.Append(",rp:" + options.RowsLength);
			scriptSrc.Append(",resizeWidth:" + options.ResizeWidth.ToString().ToLower());
			scriptSrc.Append(",method:\"" + options.HttpMethod.ToString() + "\"")
			 .Append(",sortname:\"" + options.SortName.ToString() + "\"")
			 .Append(",sortorder:\"" + options.SortOrder.ToString() + "\"")
			 .Append(",autoload:" + options.Autoload.ToString().ToLower() + "")
			 .Append(",height:" + options.Height.ToString() + "")
			 .Append("});});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}

		public static string jQueryAjaxTable(this HtmlHelper helper, string targetId, AjaxTableOptions options) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targetId + "\").ajaxTable({")
					 .Append("usepager:" + options.Paging.ToString().ToLower());
			if (string.IsNullOrEmpty(options.ControllerName) == false && string.IsNullOrEmpty(options.ActionName) == false) {
				scriptSrc.Append(",url:\"/" + options.ControllerName.ToString() + "/" + options.ActionName.ToString() + "\"");
			}
			if (string.IsNullOrEmpty(options.OnSuccess) == false) {
				scriptSrc.Append(",onSuccess:" + options.OnSuccess + "");
			}
			if (string.IsNullOrEmpty(options.OnSubmit) == false) {
				scriptSrc.Append(",onSubmit:" + options.OnSubmit + "");
			}
			if (string.IsNullOrEmpty(options.OnRowClick) == false) {
				scriptSrc.Append(",onRowClick:" + options.OnRowClick + "");
			}
			if (string.IsNullOrEmpty(options.OnRowBound) == false) {
				scriptSrc.Append(",onRowBound:" + options.OnRowBound + "");
			}
			if (string.IsNullOrEmpty(options.OnChangeSort) == false) {
				scriptSrc.Append(",onChangeSort:" + options.OnChangeSort + "");
			}
			if (options.AppendExistRows) { scriptSrc.Append(",appendExistRows:true"); }
			scriptSrc.Append(",rpOptions:[");
			string rows = string.Empty;
			foreach (var value in options.RowOptions) {
				rows += value + ",";
			}
			if (rows.Length > 0) {
				rows = rows.Substring(0, rows.Length - 1);
			}
			scriptSrc.Append(rows + "]");
			scriptSrc.Append(",rp:" + options.RowsLength);
			scriptSrc.Append(",method:\"" + options.HttpMethod.ToString() + "\"")
			 .Append(",sortname:\"" + options.SortName.ToString() + "\"")
			 .Append(",sortorder:\"" + options.SortOrder.ToString() + "\"")
			 .Append(",autoload:" + options.Autoload.ToString().ToLower() + "")
			 .Append("});});");
			return string.Format("<script  type=\"text/javascript\">{0}</script>", scriptSrc.ToString());
		}
		
		
	}
}