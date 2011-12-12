using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {
	public class TableOptions {

		public TableOptions(){
			CellPadding = 0;
			CellSpacing = 0;
			Border = 0;
			Columns = new List<TableColumnOptions>();
		}
		
		public string Name { get; set; }

		public string ID { get; set; }

		public int CellPadding { get; set; }

		public int CellSpacing { get; set; }

		public int Border { get; set; }

		public string Width { get; set; }

		public object HtmlAttributes { get; set; }

		public List<TableColumnOptions> Columns { get; set; }
	}

	public class TableColumnOptions {

		public string InnerHtml { get; set; }

		public string Name { get; set; }

		public string ID { get; set; }

		public string SortName { get; set; }

		public object HtmlAttributes { get; set; }
	}
}