using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {
	public class DataTypeHelper {

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
	 
	}
}