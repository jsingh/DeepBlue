using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Helpers {
	public class PagingDataTable : DataTable {

		public PagingDataTable() {
			this.Columns.Add(new DataColumn {
				AutoIncrement = true,
				AutoIncrementSeed = 1,
				AutoIncrementStep = 1,
				ColumnName = "RowNumber",
				AllowDBNull = false
			});
			this.PageSize = 10;
		}

		public int PageSize { get; set; }

		public int TotalPages {
			get {
				return Convert.ToInt32(Math.Ceiling(decimal.Divide((decimal)this.Rows.Count, (decimal)this.PageSize)));
			}
		}

		public PagingDataTable Skip(int pageIndex) {
			PagingDataTable filterTable = (PagingDataTable)this.Clone();
			DataRow[] rows = this.Select("RowNumber>=" + (pageIndex - 1).ToString() + " && " + "RowNumber<=" + (pageIndex * this.PageSize).ToString());
			foreach (var row in rows) {
				filterTable.ImportRow(row);
			}
			return filterTable;
		}


	}
}