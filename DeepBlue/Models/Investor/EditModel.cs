using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;
using DeepBlue.Models.Entity;

namespace DeepBlue.Models.Investor {
	public class EditModel {

		public EditModel(){
			AddressInformations = new List<AddressInformation>();
			ContactInformations = new List<ContactInformation>();
			AccountInformations = new List<AccountInformation>();
			InvestorFunds = new List<InvestorFund>();
		}
		
		public SelectListModel SelectList = new SelectListModel();

		[DisplayName("InvestorName:")]
		public string InvestorName { get; set; }

		[DisplayName("DisplayName:")]
		public string DisplayName { get; set; }

		[DisplayName("Social Security/Tax Id:")]
		public int SocialSecurityTaxId { get; set; }

		[DisplayName("State of residency:")]
		public int StateOfResidency { get; set; }

		[DisplayName("Entity Type:")]
		public int EntityType { get; set; }

		[DisplayName("Domestic/Foreign:")]
		public bool DomesticForeign { get; set; }

		public List<AddressInformation> AddressInformations { get; set; }

		public List<ContactInformation> ContactInformations { get; set; }

		public List<AccountInformation> AccountInformations { get; set; }

		public List<InvestorFund> InvestorFunds { get; set; }

		public int id { get; set; }
	}

	public class AddressInformation {

		public int AddressId { get; set; }

		[DisplayName("Telephone No:")]
		public string Phone { get; set; }

		[DisplayName("Fax No:")]
		public string Fax { get; set; }

		[DisplayName("Email:")]
		public string Email { get; set; }

		[DisplayName("Web Address:")]
		public string WebAddress { get; set; }

		[DisplayName("Address1:")]
		public string Address1 { get; set; }

		[DisplayName("Address2:")]
		public string Address2 { get; set; }

		[DisplayName("City:")]
		public string City { get; set; }

		[DisplayName("State:")]
		public int State { get; set; }

		[DisplayName("Zip:")]
		public string Zip { get; set; }

		[DisplayName("Country:")]
		public int Country { get; set; }
	}

	public class ContactInformation {

		public int ContactId { get; set; }

		public int ContactAddressId { get; set; }

		[DisplayName("Contact Person:")]
		public string ContactPerson { get; set; }

		[DisplayName("Designation:")]
		public string Designation { get; set; }

		[DisplayName("Telephone No:")]
		public string ContactPhoneNumber { get; set; }

		[DisplayName("Fax No:")]
		public string ContactFaxNumber { get; set; }

		[DisplayName("Email:")]
		public string ContactEmail { get; set; }

		[DisplayName("Web Address:")]
		public string ContactWebAddress { get; set; }

		[DisplayName("Address1:")]
		public string ContactAddress1 { get; set; }

		[DisplayName("Address2:")]
		public string ContactAddress2 { get; set; }

		[DisplayName("City:")]
		public string ContactCity { get; set; }

		[DisplayName("State:")]
		public int ContactState { get; set; }

		[DisplayName("Zip:")]
		public string ContactZip { get; set; }

		[DisplayName("Country:")]
		public int ContactCountry { get; set; }

		[DisplayName("Receives Distribution/Capital Call Notices:")]
		public bool DistributionNotices { get; set; }

		[DisplayName("Financials:")]
		public bool Financials { get; set; }

		[DisplayName("K1:")]
		public bool K1 { get; set; }

		[DisplayName("Investor Letters:")]
		public bool InvestorLetters { get; set; }
	}

	public class AccountInformation{
		
		public int AccountId { get; set; }
		
		[DisplayName("Bank Name:")]
		public string BankName { get; set; }

		[DisplayName("Account #:")]
		public string AccountNumber { get; set; }

		[DisplayName("ABA #:")]
		public string ABANumber { get; set; }

		[DisplayName("Account Of:")]
		public string AccountOf { get; set; }

		[DisplayName("FFC:")]
		public string FFC { get; set; }

		[DisplayName("FFC#:")]
		public string FFCNO { get; set; }

		[DisplayName("Swift:")]
		public string Swift { get; set; }

		[DisplayName("IBAN:")]
		public string IBAN { get; set; }

		[DisplayName("By Order Of:")]
		public string ByOrderOf { get; set; }

		[DisplayName("Reference:")]
		public string Reference { get; set; }

		[DisplayName("Attention:")]
		public string Attention { get; set; }
	}
}