using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace DeepBlue.Helpers {
	public static class FormatHelper {

		//public static string NumberFormat(int? value) {
		//    return String.Format("{0:N0}", (value ?? 0)); 
		//}


		public static string StringFormat(decimal? value, string format) {
			return ((value ?? 0) == 0 ? string.Empty : String.Format(format, (value ?? 0)).Replace("$", ""));
		}

		public static string NumberFormat(decimal? value) {
			return ((value ?? 0) == 0 ? string.Empty : String.Format("{0:C}", (value ?? 0)).Replace("$",""));
		}
		
		public static string CurrencyFormat(decimal? value) {
			return ((value ?? 0) == 0 ? string.Empty :  String.Format("{0:C}", (value ?? 0)));
		}

		public static string PercentageFormat(decimal? value) {
			return String.Format("{0:P}", (value ?? 0));
		}

		public static string PercentageFormat(int? value) {
			return String.Format("{0:P0}", (value ?? 0));
		}
	}
}