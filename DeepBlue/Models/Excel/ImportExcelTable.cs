using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Excel {

	public class ImportExcelTableModel {

		public ImportExcelTableModel() {
			this.TableName = string.Empty;
			this.Columns = new List<string>();
			this.TotalRows = 0;
		}

		public string TableName { get; set; }

		public List<string> Columns { get; set; }

		public int TotalRows { get; set; }

		public string SessionKey { get; set; }
	}

	public class ImportExcelModel {

		public string Result { get; set; }

		public List<ImportExcelTableModel> Tables { get; set; }

	}
}