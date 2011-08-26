using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Helpers {
	public class EmailAttribute : RegularExpressionAttribute {
		public EmailAttribute()
			: base("\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*") {		}
	}
}