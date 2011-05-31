using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace DeepBlue.Helpers {
	public static class FormatHelper {

		public static string NumberFormat(int value) {
			NumberFormatInfo numberFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
			numberFormat.CurrencySymbol = string.Empty;
			numberFormat.NumberDecimalDigits = 0;
			numberFormat.CurrencyDecimalDigits = 0;
			return value.ToString("c", numberFormat);
		}
	}
}