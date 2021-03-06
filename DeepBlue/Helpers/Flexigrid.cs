﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DeepBlue.Helpers {

	public class FlexigridRow {
		public List<object> cell = new List<object>();


		public object Add {
			set {
				cell.Add(value);
			}
		}
	}

	public class FlexigridData {
		public int page;
		public int total;
		public List<FlexigridRow> rows = new List<FlexigridRow>();
	}

	public class GridData {
		public int page;
		public int total;
		public IEnumerable<object> rows;
	}
}