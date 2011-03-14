using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {
	public class DatePickerOptions {
		public DatePickerOptions() {
			OnBeforeShow = string.Empty;
			OnClose = string.Empty;
			OnSelect = string.Empty;
		}
		public string OnBeforeShow { get; set; }
		public string OnClose { get; set; }
		public string OnSelect { get; set; }
	}
}