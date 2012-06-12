using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Helpers {
	public static class DataTypeHelper {

		public static decimal ToDecimal(string value) {
			decimal returnValue;
			decimal.TryParse(value, out returnValue);
			return returnValue;
		}

		public static Int32 ToInt32(string value) {
			int returnValue;
			Int32.TryParse(value, out returnValue);
			return returnValue;
		}

		public static DateTime ToDateTime(string value) {
			DateTime returnValue;
			DateTime.TryParse(value, out returnValue);
			return returnValue.Year <= 1900 ? new DateTime(1900,1,1) : returnValue;
		}

		public static bool CheckBoolean(string value) {
		  	return (string.IsNullOrEmpty(value) ? false : value.Contains("true"));
		}

		public static string GetValue(this DataRow row, string columnName) {
			string value = string.Empty;
			if (string.IsNullOrEmpty(columnName) == false) {
				columnName = columnName.Trim();
				if (string.IsNullOrEmpty(columnName) == false) {
					try {
						value = row[columnName].ToString();
					}
					catch { }
				}
			}
			return value;
		}
	 
	}
}