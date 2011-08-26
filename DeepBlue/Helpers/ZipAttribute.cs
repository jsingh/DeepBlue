using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Helpers {
	public class ZipAttribute : RegularExpressionAttribute {
		public ZipAttribute()
			: base("^(\\d{5}-\\d{4}|\\d{5}|\\d{9})$|^([a-zA-Z]\\d[a-zA-Z]\\d[a-zA-Z]\\d)$") { }
	}
}