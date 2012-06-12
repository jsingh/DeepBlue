using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;

namespace DeepBlue.Helpers {
	public  class ExcelConnection {

		#region Constants
		private const string EXCELDATABASE_BY_KEY = "ExcelDatabase-{0}";
		#endregion

		public static DataSet GetDataSet(string path, string fileName, ref string errorMessage, ref string sessionKey) {
			ICacheManager cacheManager = new MemoryCacheManager();
			DataSet ds = new DataSet();
			PagingDataTable table = null;
			try {
				//string connectionString = "provider=Microsoft.ACE.OLEDB.12.0;data source='" + fileName + "';Extended Properties=Excel 12.0;";
				string connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES\";", fileName);
				using (OleDbConnection connection = new OleDbConnection(connectionString)) {
					connection.Open();
					DataTable schema = connection.GetSchema("Tables");
					foreach (DataRow row in schema.Rows) {
						string tableName = row["TABLE_NAME"].ToString();
						string tableDisplayName = tableName.Replace("$", "").Replace("'", "");
						using (OleDbCommand command = new OleDbCommand(string.Format("select * from [{0}]", tableName), connection)) {
							table = new PagingDataTable();
							table.TableName = tableDisplayName;
							table.Load(command.ExecuteReader());
							ds.Tables.Add(table);
						}
					}
					Guid guid = System.Guid.NewGuid();
					sessionKey = string.Format(EXCELDATABASE_BY_KEY, guid);
					cacheManager.Set(sessionKey, ds, 15);
				}
			}
			catch (Exception ex) {
				errorMessage = ex.Message.ToString();
			}
			return ds;
		}

		public static DataSet ImportExcelDataset(string key) {
			ICacheManager cacheManager = new MemoryCacheManager();
			return cacheManager.Get<DataSet>(key);
		}

	}
}