using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DeepBlue.Helpers {
	public class DateRange : ValidationAttribute {

		public DateRange() {
			Minimum = Convert.ToDateTime("01/01/1900");
			Maximum = DateTime.MaxValue;
		}

		public DateRange(DateTime minimum, DateTime maximum) {
			Minimum = minimum;
			Maximum = maximum;
		}

		public DateTime Maximum { get; private set; }
		public DateTime Minimum { get; private set; }

		public override string FormatErrorMessage(string name) {
			return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
		}
		public override bool IsValid(object value) {
			if (value != null) {
				DateTime date;
				DateTime.TryParse(value.ToString(), out date);
				if ((date - this.Minimum).TotalDays < 0)
					return false;
				else if ((this.Maximum - date).TotalDays < 0)
					return false;
				else
					return true;
			}
			return false;
		}
	}
}