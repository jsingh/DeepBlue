using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Deal {
	public class AccountInformationModel {

		public int? AccountId { get; set; }

		[StringLength(50, ErrorMessage = "Bank Name must be under 50 characters.")]
		[DisplayName("Bank Name")]
		public string BankName { get; set; }

		[StringLength(40, ErrorMessage = "Account Name must be under 40 characters.")]
		[DisplayName("Account Name")]
		public string Account { get; set; }

		[StringLength(50, ErrorMessage = "Account Number No must be under 50 characters.")]
		[DisplayName("Account Number")]
		public string AccountNumber { get; set; }

		[StringLength(50, ErrorMessage = "Account Of must be under 50 characters.")]
		[DisplayName("Account Of")]
		public string AccountOf { get; set; }

		[StringLength(50, ErrorMessage = "Reference must be under 50 characters.")]
		[DisplayName("Reference")]
		public string Reference { get; set; }

		[StringLength(50, ErrorMessage = "Attention must be under 50 characters.")]
		[DisplayName("Attention")]
		public string Attention { get; set; }

		[StringLength(50, ErrorMessage = "Swift Code must be under 50 characters.")]
		[DisplayName("Swift")]
		public string Swift { get; set; }

		[StringLength(50, ErrorMessage = "FFC must be under 50 characters.")]
		[DisplayName("FFC Name")]
		public string FFC { get; set; }

		[StringLength(50, ErrorMessage = "FFC Number must be under 50 characters.")]
		[DisplayName("FFCNumber")]
		public string FFCNumber { get; set; }

		[StringLength(50, ErrorMessage = "IBAN Number must be under 50 characters.")]
		[DisplayName("IBAN")]
		public string IBAN { get; set; }

		[StringLength(50, ErrorMessage = "Telephone Number must be under 50 characters.")]
		[DisplayName("Phone")]
		public string AccountPhone { get; set; }
	 
		[DisplayName("ABA Number")]
		public int? ABANumber { get; set; }

		[StringLength(50, ErrorMessage = "Fax must be under 50 characters.")]
		[DisplayName("Fax")]
		public string AccountFax { get; set; }

		[StringLength(50, ErrorMessage = "ByOrderOf must be under 50 characters.")]
		[DisplayName("ByOrderOf")]
		public string ByOrderOf { get; set; }

	}
}