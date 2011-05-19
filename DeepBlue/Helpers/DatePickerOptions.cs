using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {
	public class DatePickerOptions {
		public DatePickerOptions() {
			OnCreate = string.Empty;
			OnBeforeShow = string.Empty;
			OnBeforeShowDay = string.Empty;
			OnClose = string.Empty;
			OnSelect = string.Empty;
			OnChangeMonthYear = string.Empty;
		}
		public string OnCreate { get; set; }
		public string OnBeforeShow { get; set; }
		public string OnBeforeShowDay { get; set; }
		public string OnChangeMonthYear { get; set; }
		public string OnClose { get; set; }
		public string OnSelect { get; set; }
	}
}