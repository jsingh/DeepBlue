using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DeepBlue.Helpers {

	public class DateRangeAttribute : ValidationAttribute {
		private const string DateFormat = "MM/dd/yyyy";
		private const string DefaultErrorMessage =
			   "'{0}' must be a date between {1:d} and {2:d}.";

		public DateTime MinDate { get; set; }
		public DateTime MaxDate { get; set; }

		public DateRangeAttribute()
			: base(DefaultErrorMessage) {
			MinDate = ParseDate("01/01/1900");
			MaxDate = DateTime.MaxValue;
		}

		public DateRangeAttribute(string minDate, string maxDate)
			: base(DefaultErrorMessage) {
			MinDate = ParseDate(minDate);
			MaxDate = ParseDate(maxDate);
		}

		public override bool IsValid(object value) {
			if (value == null || !(value is DateTime)) {
				return true;
			}
			DateTime dateValue = (DateTime)value;
			return MinDate <= dateValue && dateValue <= MaxDate;
		}
		public override string FormatErrorMessage(string name) {
			return String.Format(CultureInfo.CurrentCulture,
				ErrorMessageString,
				name, MinDate, MaxDate);
		}

		private static DateTime ParseDate(string dateValue) {
			return DateTime.ParseExact(dateValue, DateFormat,
				 CultureInfo.InvariantCulture);
		}
	}
}