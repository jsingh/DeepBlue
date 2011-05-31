using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {
	public class AutoCompleteOptions {
		public AutoCompleteOptions() {
			MinLength = 2;
			Delay = 300;
			AppendTo = "body";
			Disabled = false;
			Source = string.Empty;
			OnSelect = string.Empty;
			OnChange =  string.Empty;
			OnSearch = string.Empty;
		}
		public int MinLength { get; set; }
		public string Source { get; set; }
		public int Delay { get; set; }
		public string AppendTo { get; set; }
		public bool Disabled { get; set; }
		public string OnSearch { get; set; }
		public string OnSelect { get; set; }
		public string OnChange { get; set; }
	}
	public class AutoCompleteList{
		public AutoCompleteList(){
			id = 0;
			label = string.Empty;
			value = string.Empty;
		}
		public int id { get; set; }
		public string label { get; set; }
		public string value { get; set; }
	}
}