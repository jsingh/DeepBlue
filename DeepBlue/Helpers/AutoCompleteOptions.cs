using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {
	public class AutoCompleteOptions {
		public AutoCompleteOptions() {
			MinLength = 1;
			Delay = 300;
			AppendTo = "body";
			Disabled = false;
			Source = string.Empty;
			SearchFunction = string.Empty;
			OnSelect = string.Empty;
			OnChange = string.Empty;
			OnSearch = string.Empty;
		}
		public int MinLength { get; set; }
		public string Source { get; set; }
		public string SearchFunction { get; set; }
		public int Delay { get; set; }
		public string AppendTo { get; set; }
		public bool Disabled { get; set; }
		public string OnSearch { get; set; }
		public string OnSelect { get; set; }
		public string OnChange { get; set; }
		public static int RowsLength { get { return 200; } }
	}

	public class AutoCompleteList {
		public AutoCompleteList() {
			id = 0;
			label = string.Empty;
			value = string.Empty;
			otherid = 0;
			otherid2 = 0;
		}
		public int id { get; set; }
		public string label { get; set; }
		public string value { get; set; }
		public int otherid { get; set; }
		public int otherid2 { get; set; }
	}


}