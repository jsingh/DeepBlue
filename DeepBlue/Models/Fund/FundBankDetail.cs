using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DeepBlue.Models.Fund {
	public class FundBankDetail {

		/* Bank Details */

		public int? AccountId { get; set; }

		[StringLength(50, ErrorMessage = "Bank Name must be under 50 characters.")]
		[DisplayName("Bank Name")]
		public string BankName { get; set; }

		[StringLength(40, ErrorMessage = "Account must be under 40 characters.")]
		[DisplayName("Account")]
		public string AccountNo { get; set; }

		[DisplayName("ABA Number")]
		public int? ABANumber { get; set; }

		[StringLength(50, ErrorMessage = "Swift Code must be under 50 characters.")]
		[DisplayName("Swift Code")]
		public string Swift { get; set; }

		[StringLength(50, ErrorMessage = "Account Number Cash must be under 50 characters.")]
		[DisplayName("Account Number Cash")]
		public string AccountNumberCash { get; set; }

		[StringLength(50, ErrorMessage = "FFC Number must be under 50 characters.")]
		[DisplayName("FFC Number")]
		public string FFCNumber { get; set; }

		[StringLength(50, ErrorMessage = "IBAN must be under 50 characters.")]
		[DisplayName("IBAN")]
		public string IBAN { get; set; }

		[StringLength(50, ErrorMessage = "Reference must be under 50 characters.")]
		[DisplayName("Reference")]
		public string Reference { get; set; }

		[StringLength(50, ErrorMessage = "Account Of must be under 50 characters.")]
		[DisplayName("Account Of")]
		public string AccountOf { get; set; }

		[StringLength(50, ErrorMessage = "Attention must be under 50 characters.")]
		[DisplayName("Attention")]
		public string Attention { get; set; }

		[StringLength(50, ErrorMessage = "Telephone must be under 50 characters.")]
		[DisplayName("Telephone")]
		public string Telephone { get; set; }

		[StringLength(50, ErrorMessage = "Fax must be under 50 characters.")]
		[DisplayName("Fax")]
		public string Fax { get; set; }
	}
}