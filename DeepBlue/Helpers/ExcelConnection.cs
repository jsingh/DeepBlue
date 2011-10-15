using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;

namespace DeepBlue.Helpers {
	public class ExcelConnection {

		public static PagingDataTable GetTable(string path, string fileName, string errorMessage) {
			PagingDataTable table = null;
			try {
				string connectionString = "provider=Microsoft.Jet.OLEDB.4.0;data source='" + fileName + "';Extended Properties=Excel 8.0;";
				using (OleDbConnection connection = new OleDbConnection(connectionString)) {
					connection.Open();
					using (OleDbCommand command = new OleDbCommand("select * from [sheet1$]", connection)) {
						table = new PagingDataTable();
						table.Load(command.ExecuteReader());
					}
				}
			}
			catch (Exception ex) {
				errorMessage = ex.Message.ToString();
			}
			return table;
		}

	}
}