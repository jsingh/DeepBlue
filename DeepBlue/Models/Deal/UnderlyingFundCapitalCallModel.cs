using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DeepBlue.Models.Deal {
	public class UnderlyingFundCapitalCallModel : UnderlyingFundActivityFields {

		public int UnderlyingFundCapitalCallId { get; set; }

		[DisplayName("Deemed Capital Call:")]
		public bool? IsDeemedCapitalCall { get; set; }

	}

	public class UnderlyingFundCapitalCallList : UnderlyingFundActivityFields {

		public int UnderlyingFundCapitalCallId { get; set; }

		public bool? IsDeemedCapitalCall { get; set; }

	}
}