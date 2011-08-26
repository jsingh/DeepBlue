using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace DeepBlue.Models.Investor {
	public class BankDetail {

		/* Bank Information */

		[DisplayName("Bank Name")]
		public string BankName { get; set; }

		[DisplayName("Account #")]
		public string AccountNumber { get; set; }

		[DisplayName("ABA #")]
		public string ABANumber { get; set; }

		[DisplayName("Account Of")]
		public string AccountOf { get; set; }

		[DisplayName("FFC")]
		public string FFC { get; set; }

		[DisplayName("FFC#")]
		public string FFCNO { get; set; }

		[DisplayName("Swift")]
		public string Swift { get; set; }

		[DisplayName("IBAN")]
		public string IBAN { get; set; }

		[DisplayName("By Order Of")]
		public string ByOrderOf { get; set; }

		[DisplayName("Reference")]
		public string Reference { get; set; }

		[DisplayName("Attention")]
		public string Attention { get; set; }
	}
}