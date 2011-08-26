using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Helpers {
	public class WebAddressAttribute: RegularExpressionAttribute {
		public WebAddressAttribute()
			: base("(http|https)://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?") {		}
	}
}