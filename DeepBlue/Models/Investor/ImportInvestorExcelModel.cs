using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue.Models.Deal;

namespace DeepBlue.Models.Investor {

	public class ImportInvestorExcelModel : ImportExcelPagingModel {

		public string InvestorTableName { get; set; }

		public string InvestorName { get; set; }

		public string DisplayName { get; set; }

		public string SocialSecurityID { get; set; }

		public string DomesticForeign { get; set; }

		public string StateOfResidency { get; set; }

		public string EntityType { get; set; }

		public string Source { get; set; }

		public string FOIA { get; set; }

		public string ERISA { get; set; }

		public string Notes { get; set; }

		public string Phone { get; set; }

		public string Fax { get; set; }

		public string Email { get; set; }

		public string WebAddress { get; set; }

		public string Address1 { get; set; }

		public string Address2 { get; set; }

		public string City { get; set; }

		public string State { get; set; }

		public string Zip { get; set; }

		public string Country { get; set; }

	}

	public class ImportInvestorBankExcelModel : ImportExcelPagingModel {

		public string InvestorBankTableName { get; set; }

		public string InvestorName { get; set; }

		public string BankName { get; set; }

		public string ABANumber { get; set; }

		public string AccountName { get; set; }

		public string AccountNumber { get; set; }

		public string FFCName { get; set; }

		public string FFCNumber { get; set; }

		public string Reference { get; set; }

		public string Swift { get; set; }

		public string IBAN { get; set; }

		public string Phone { get; set; }

		public string Fax { get; set; }

	}

	public class ImportInvestorContactExcelModel : ImportExcelPagingModel {

		public string InvestorContactTableName { get; set; }

		public string InvestorName { get; set; }

		public string ContactPerson { get; set; }

		public string Designation { get; set; }

		public string Telephone { get; set; }

		public string Fax { get; set; }

		public string Email { get; set; }

		public string WebAddress { get; set; }

		public string Address { get; set; }

		public string City { get; set; }

		public string State { get; set; }

		public string Zip { get; set; }

		public string Country { get; set; }

		public string ReceivesDistributionCapitalCallNotices { get; set; }

		public string Financials { get; set; }

		public string InvestorLetters { get; set; }

	}
}