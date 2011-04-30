using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {
	public class AjaxTableOptions {
		public AjaxTableOptions() {
			ActionName = string.Empty;
			ControllerName = string.Empty;
			RowOptions = new int[] { 15, 20, 50, 100 };
			RowsLength = 15;
			OnSuccess = string.Empty;
			OnSubmit = string.Empty;
			OnRowClick = string.Empty;
			OnRowBound = string.Empty;
			OnChangeSort = string.Empty;
			HttpMethod = "GET";
			SortName = string.Empty;
			SortOrder = "asc";
			Autoload = true;
			AppendExistRows = false;
		}
		public string HttpMethod { get; set; }
		public string ActionName { get; set; }
		public string ControllerName { get; set; }
		public bool Paging { get; set; }
		public int[] RowOptions { get; set; }
		public int RowsLength { get; set; }
		public string OnSuccess { get; set; }
		public string OnSubmit { get; set; }
		public string OnRowClick { get; set; }
		public string OnRowBound { get; set; }
		public string SortName  { get; set; }
		public string SortOrder { get; set; }
		public bool Autoload  { get; set; }
		public bool AppendExistRows { get; set; }
		public string OnChangeSort { get; set; }
	}
}