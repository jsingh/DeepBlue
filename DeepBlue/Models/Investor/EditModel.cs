using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Investor {
	public class EditModel {
		public EditModel() {
			AddressInformations = new List<AddressInformation>();
			ContactInformations = new List<ContactInformation>();
			AccountInformations = new List<AccountInformation>();
			FundInformations = new FlexigridObject();
			InvestorName = string.Empty;
			DisplayName = string.Empty;
			Notes = string.Empty;
			SocialSecurityTaxId = string.Empty;
			EntityType = 0;
			DomesticForeign = false;
		}

		public SelectListModel SelectList = new SelectListModel();

		[DisplayName("InvestorName:")]
		public string InvestorName { get; set; }

		[DisplayName("DisplayName:")]
		public string DisplayName { get; set; }

		[DisplayName("Notes:")]
		public string Notes { get; set; }
		
		[DisplayName("Social Security/Tax Id:")]
		public string SocialSecurityTaxId { get; set; }

		[DisplayName("State of Residency:")]
		public int StateOfResidency { get; set; }

		[DisplayName("Entity Type:")]
		public int EntityType { get; set; }

		[DisplayName("Domestic/Foreign:")]
		public bool DomesticForeign { get; set; }

		public List<AddressInformation> AddressInformations { get; set; }

		public List<ContactInformation> ContactInformations { get; set; }

		public List<AccountInformation> AccountInformations { get; set; }

		public FlexigridObject FundInformations { get; set; }

		public int id { get; set; }
	}

	public class AddressInformation {
		public AddressInformation() {
			AddressId = 0;
			Phone = string.Empty;
			Fax = string.Empty;
			Email = string.Empty;
			WebAddress = string.Empty;
			Address1 = string.Empty;
			Address2 = string.Empty;
			City = string.Empty;
			State = 0;
			Zip = string.Empty;
			Country = 0;
		}

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
		public ContactInformation(){
			ContactId = 0;
			ContactAddressId = 0;
			ContactPerson = string.Empty;
			Designation = string.Empty;
			ContactPhoneNumber = string.Empty;
			ContactFaxNumber = string.Empty;
			ContactEmail = string.Empty;
			ContactWebAddress = string.Empty;
			ContactAddress1 = string.Empty;
			ContactAddress2 = string.Empty;
			ContactCity = string.Empty;
			ContactState = 0;
			ContactZip = string.Empty;
			ContactCountry = 0;
			DistributionNotices = false;
			Financials = false;
			K1 = false;
			InvestorLetters = false;
		}

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

	public class AccountInformation {
		public AccountInformation(){
			AccountId = 0;
			BankName = string.Empty;
			AccountNumber = string.Empty;
			ABANumber = string.Empty;
			AccountOf = string.Empty;
			FFC = string.Empty;
			FFCNO = string.Empty;
			Swift = string.Empty;
			IBAN = string.Empty;
			ByOrderOf = string.Empty;
			Reference = string.Empty;
			Attention = string.Empty;
		}

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

	public class FundInformation {
		public FundInformation(){
			FundName = string.Empty;
			TotalCommitment = 0;
			UnfundedAmount = 0;
			InvestorType = string.Empty;
		}
		public string FundName { get; set; }

		public decimal TotalCommitment { get; set; }

		public decimal UnfundedAmount { get; set; }

		public string InvestorType { get; set; }
	}
}