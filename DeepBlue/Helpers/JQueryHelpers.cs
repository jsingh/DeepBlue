﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace DeepBlue.Helpers {
	public static class JQueryHelpers {

		public static string jQueryAutoComplete(this HtmlHelper helper, string targetId, AutoCompleteOptions options) {
			StringBuilder scriptSrc = new StringBuilder();
			scriptSrc.Append("$(document).ready(function(){$(\"#" + targetId + "\").autocomplete({");
			if (string.IsNullOrEmpty(options.Source) == false) {
				if (options.Source.StartsWith("~") == false) {
					options.Source = "~/" + options.Source;
				}
				scriptSrc.AppendFormat("source:\"{0}\"", HtmlControls.Url(helper, options.Source));
			}
			if (string.IsNullOrEmpty(options.SearchFunction) == false) {
				scriptSrc.AppendFormat("source:{0}", options.SearchFunction);
			}
			scriptSrc.Append(",minLength:" + options.MinLength.ToString())
			.Append(",autoFocus: true")
			.Append((string.IsNullOrEmpty(options.OnSelect) ? "" : ",select:" + options.OnSelect.ToString()))
			.Append((string.IsNullOrEmpty(options.OnChange) ? "" : ",change:" + options.OnChange.ToString()))
			.Append((string.IsNullOrEmpty(options.OnSearch) ? "" : ",search:" + options.OnSearch.ToString()))
			.Append(",appendTo:\"" + options.AppendTo + "\"")
			.Append(",delay:" + options.Delay.ToString());
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
					 .Append("usepager:" + options.Paging.ToString().ToLower())
					 .Append(",useBoxStyle:" + options.BoxStyle.ToString().ToLower());
			if (string.IsNullOrEmpty(options.ControllerName) == false && string.IsNullOrEmpty(options.ActionName) == false) {
				scriptSrc.AppendFormat(",url:\"{0}\"", HtmlControls.Url(helper, options.ActionName, options.ControllerName));
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
			if (string.IsNullOrEmpty(options.OnInit) == false) {
				scriptSrc.Append(",onInit:" + options.OnInit + "");
			}
			if (string.IsNullOrEmpty(options.OnTemplate) == false) {
				scriptSrc.Append(",onTemplate:" + options.OnTemplate + "");
			}
			if (string.IsNullOrEmpty(options.TableName) == false) {
				scriptSrc.Append(",tableName:'" + options.TableName + "'");
			}
			if (options.ExportExcel) {
				scriptSrc.Append(",exportExcel:" + options.ExportExcel.ToString().ToLower() + "");
			}
			if (options.Width > 0) {
				scriptSrc.Append(",width:" + options.Width);
			}
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
				scriptSrc.AppendFormat(",url:\"{0}\"", HtmlControls.Url(helper, options.ActionName, options.ControllerName));
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
			if (string.IsNullOrEmpty(options.RowClass) == false) {
				scriptSrc.Append(",rowClass:'" + options.RowClass + "'");
			}
			if (string.IsNullOrEmpty(options.AlternateRowClass) == false) {
				scriptSrc.Append(",alternateRowClass:'" + options.AlternateRowClass + "'");
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