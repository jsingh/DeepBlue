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

		public static string NumberFormat(decimal? value) {
			return String.Format("{0:0,##;(0,##);\\}", (value ?? 0));
		}
		
		public static string CurrencyFormat(decimal? value) {
			return String.Format("{0:$0,##;$(0,##);\\}", (value ?? 0));
		}

		public static string PercentageFormat(decimal? value) {
			return String.Format("{0:P}", (value ?? 0));
		}

		public static string PercentageFormat(int? value) {
			return String.Format("{0:P0}", (value ?? 0));
		}
	}
}