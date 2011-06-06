using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DeepBlue.Models.Deal {
	public class UnderlyingFundCapitalCallModel : UnderlyingFundActivityFields {

		public UnderlyingFundCapitalCallModel() {
			UnderlyingFundCapitalCallId = 0;
		}

		public int? UnderlyingFundCapitalCallId { get; set; }

		[DisplayName("Deemed Capital Call:")]
		public bool? IsDeemedCapitalCall { get; set; }

	}
 
}