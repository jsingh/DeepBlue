using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {

	public class FlexigridRow {
		public string id;
		public List<string> cell = new List<string>();
	}

	public class FlexigridObject {
		public int page;
		public int total;
		public List<FlexigridRow> rows = new List<FlexigridRow>();
	}
}