using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DeepBlue.Helpers {
	public class ExcelConnection {

		#region Constants
		private const string EXCELDATABASE_BY_KEY="ExcelDatabase-{0}";
		#endregion

		public static DataSet GetDataSet(string path,string fileName,ref string errorMessage,ref string sessionKey) {
			ICacheManager cacheManager=new MemoryCacheManager();
			DataSet ds=new DataSet();
			PagingDataTable dt=null;
			try {
				string inputFileName=System.IO.Path.Combine(path,fileName);
				using(SpreadsheetDocument myWorkbook=SpreadsheetDocument.Open(inputFileName,false)) {
					//Access the main Workbook part, which contains data
					WorkbookPart workbookPart=myWorkbook.WorkbookPart;
					WorksheetPart worksheetPart=null;
					List<Sheet> sheets=workbookPart.Workbook.Descendants<Sheet>().ToList();
					foreach(var ss in sheets) {
						dt=new PagingDataTable();
						dt.TableName=ss.Name;
						worksheetPart=(WorksheetPart)workbookPart.GetPartById(ss.Id);
						SharedStringTablePart stringTablePart=workbookPart.SharedStringTablePart;
						if(worksheetPart!=null) {
							Row lastRow=worksheetPart.Worksheet.Descendants<Row>().LastOrDefault();
							Row firstRow=worksheetPart.Worksheet.Descendants<Row>().FirstOrDefault();
							if(firstRow!=null) {
								foreach(Cell c in firstRow.ChildElements) {
									string value=GetValue(c,stringTablePart);
									dt.Columns.Add(value);
								}
							}
							if(lastRow!=null) {
								for(int i=2;i<=lastRow.RowIndex;i++) {
									DataRow dr=dt.NewRow();
									bool empty=true;
									Row row=worksheetPart.Worksheet.Descendants<Row>().Where(r => i==r.RowIndex).FirstOrDefault();
									int j=2;
									if(row!=null) {
										foreach(Cell c in row.ChildElements) {
											//Get cell value
											string value=GetValue(c,stringTablePart);
											if(!string.IsNullOrEmpty(value)&&value!="")
												empty=false;

											dr[j]=value;
											j++;

											if(j==dt.Columns.Count)
												break;
										}
										if(empty)
											break;

										dt.Rows.Add(dr);
									}
								}
							}
						}
						ds.Tables.Add(dt);
					}
				}
				Guid guid=System.Guid.NewGuid();
				sessionKey=string.Format(EXCELDATABASE_BY_KEY,guid);
				cacheManager.Set(sessionKey,ds,60);
				if(UploadFileHelper.TempFileExist(inputFileName))
					UploadFileHelper.TempFileDelete(inputFileName);
			} catch(Exception ex) {
				errorMessage=ex.Message.ToString();
			}

			return ds;
		}

		private static string GetValue(Cell cell,SharedStringTablePart stringTablePart) {
			if(cell.ChildElements.Count==0) return null;
			//get cell value
			string value=cell.ElementAt(0).InnerText;//CellValue.InnerText;
			//Look up real value from shared string table
			if((cell.DataType!=null)&&(cell.DataType==CellValues.SharedString))
				value=stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;

			return value;
		}

		public static DataSet ImportExcelDataset(string key) {
			ICacheManager cacheManager=new MemoryCacheManager();
			return cacheManager.Get<DataSet>(key);
		}

		/*
		public static DataSet GetDataSet_(string path,string fileName,ref string errorMessage,ref string sessionKey) {
			ICacheManager cacheManager=new MemoryCacheManager();
			DataSet ds=new DataSet();
			PagingDataTable table=null;
			try {
				//string connectionString = "provider=Microsoft.ACE.OLEDB.12.0;data source='" + fileName + "';Extended Properties=Excel 12.0;";

				string connectionString=string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES\";",fileName);
				using(OleDbConnection connection=new OleDbConnection(connectionString)) {
					connection.Open();
					DataTable schema=connection.GetSchema("Tables");
					foreach(DataRow row in schema.Rows) {
						string tableName=row["TABLE_NAME"].ToString();
						string tableDisplayName=tableName.Replace("$","").Replace("'","");
						using(OleDbCommand command=new OleDbCommand(string.Format("select * from [{0}]",tableName),connection)) {
							table=new PagingDataTable();
							table.TableName=tableDisplayName;
							table.Load(command.ExecuteReader());
							ds.Tables.Add(table);
						}
					}
					Guid guid=System.Guid.NewGuid();
					sessionKey=string.Format(EXCELDATABASE_BY_KEY,guid);
					cacheManager.Set(sessionKey,ds,15);
				}
			} catch(Exception ex) {
				errorMessage=ex.Message.ToString();
			}
			return ds;
		}
		*/
	}
}