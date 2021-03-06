﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {
	public class FlexigridOptions {
		public FlexigridOptions() {
			Height = 0;
			Width = 0;
			ActionName = string.Empty;
			ControllerName = string.Empty;
			Paging = false;
			RowOptions = new int[] { 25, 50, 100, 200 };
			RowsLength = 25;
			OnSuccess = string.Empty;
			OnSubmit = string.Empty;
			OnRowClick = string.Empty;
			OnRowBound = string.Empty;
			HttpMethod = "GET";
			SortName = string.Empty;
			SortOrder = string.Empty;
			Autoload = true;
			ResizeWidth = true;
			OnInit = string.Empty;
			OnTemplate = string.Empty;
			TableName = string.Empty;
			ExportExcel = false;
			BoxStyle = false;
		}
		public string HttpMethod { get; set; }
		public int Height { get; set; }
		public int Width { get; set; }
		public string ActionName { get; set; }
		public string ControllerName { get; set; }
		public bool Paging { get; set; }
		public bool BoxStyle { get; set; }
		public int[] RowOptions { get; set; }
		public int RowsLength { get; set; }
		public string OnSuccess { get; set; }
		public string OnSubmit { get; set; }
		public string OnRowClick { get; set; }
		public string OnRowBound { get; set; }
		public string OnInit { get; set; }
		public string OnTemplate { get; set; }
		public string SortName { get; set; }
		public string SortOrder { get; set; }
		public bool Autoload { get; set; }
		public bool ResizeWidth { get; set; }
		public string TableName { get; set; }
		public bool ExportExcel { get; set; }
	}
}