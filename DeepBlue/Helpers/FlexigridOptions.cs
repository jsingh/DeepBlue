using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {
	public class FlexigridOptions {
		public FlexigridOptions() {
			Height = string.Empty;
			Width = string.Empty;
			ActionName = string.Empty;
			ControllerName = string.Empty;
			Paging = false;
			RowOptions = new int[] { 20, 50, 100 };
			RowsLength = 20;
			OnSuccess = string.Empty;
			OnSubmit = string.Empty;
			HttpMethod = "GET";
			SortName = string.Empty;
			SortOrder = string.Empty;
			Autoload = true;
		}
		public string HttpMethod { get; set; }
		public string Height { get; set; }
		public string Width { get; set; }
		public string ActionName { get; set; }
		public string ControllerName { get; set; }
		public bool Paging { get; set; }
		public int[] RowOptions { get; set; }
		public int RowsLength { get; set; }
		public string OnSuccess { get; set; }
		public string OnSubmit { get; set; }
		public string SortName  { get; set; }
		public string SortOrder { get; set; }
		public bool Autoload  { get; set; }
	}
}