using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;

namespace DeepBlue.Helpers {
	public class ExcelConnection {
		
		private const string import = "ImportExcelTable";

		public static PagingDataTable GetTable(string path, string fileName, ref string errorMessage) {
			PagingDataTable table = null;
			try {
				//string connectionString = "provider=Microsoft.ACE.OLEDB.12.0;data source='" + fileName + "';Extended Properties=Excel 12.0;";
				string connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES\";", fileName);
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

		public static PagingDataTable ImportExcelTable {
			get {
				if (SecurityContainer.GetHttpContext().Session[import] != null)
					return (PagingDataTable)SecurityContainer.GetHttpContext().Session[import];
				else
					return null;
			}
			set {
				SecurityContainer.GetHttpContext().Session[import] = value;
			}
		}

	}
}