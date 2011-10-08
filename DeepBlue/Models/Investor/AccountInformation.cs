using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Deal;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Investor {

	public class AccountInformation : AccountInformationModel {
		public AccountInformation() {
			AccountId = 0;
			BankName = string.Empty;
			AccountNumber = string.Empty;
			AccountOf = string.Empty;
			FFC = string.Empty;
			FFCNumber = string.Empty;
			Swift = string.Empty;
			IBAN = string.Empty;
			ByOrderOf = string.Empty;
			Reference = string.Empty;
			Attention = string.Empty;
		}

		[Required(ErrorMessage = "Investor is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Investor is required")]
		public int InvestorId { get; set; }

	}
}