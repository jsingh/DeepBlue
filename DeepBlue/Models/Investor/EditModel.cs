using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Models.Admin.Enums;
using DeepBlue.Models.Deal;

namespace DeepBlue.Models.Investor {

	public class EditModel : InvestorInformation {

		public EditModel() {
			AddressInformations = new List<AddressInformation>();
			ContactInformations = new List<ContactInformation>();
			AccountInformations = new List<AccountInformation>();
			FundInformations = new FlexigridData();
			InvestorName = string.Empty;
			Notes = string.Empty;
			SocialSecurityTaxId = string.Empty;
			EntityType = 0;
			DomesticForeign = false;
		}

		public object AddressInformations { get; set; }

		public object ContactInformations { get; set; }

		public object AccountInformations { get; set; }

		public object FundInformations { get; set; }

		public int id { get; set; }

		[Required(ErrorMessage = "Investor is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Investor is required")]
		public override int InvestorId { get; set; }

	}

	public class InvestorInformation  {

		[DisplayName("InvestorName")]
		[Required(ErrorMessage = "Investor Name is required")]
		[StringLength(100, ErrorMessage = "Investor Name must be under 30 characters.")]
		public string InvestorName { get; set; }
		 
		[DisplayName("FOIA")]
		public bool FOIA { get; set; }

		[DisplayName("Source")]
		public int Source { get; set; }

		[DisplayName("ERISA")]
		public bool ERISA { get; set; }

		[DisplayName("Notes")]
		public string Notes { get; set; }

		[DisplayName("Social Security/Tax Id")]
		public string SocialSecurityTaxId { get; set; }

		[Required(ErrorMessage = "State Of Residency is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "State Of Residency is required")]
		[DisplayName("State of Residency")]
		public int? StateOfResidency { get; set; }

		[Required(ErrorMessage = "Entity Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Entity Type is required")]
		[DisplayName("EntityType")]
		public int EntityType { get; set; }

		public virtual int InvestorId { get; set; }

		[DisplayName("Domestic/Foreign")]
		public bool DomesticForeign { get; set; }

		public string DomesticForeignName { get; set; }

		public string EntityTypeName { get; set; }

		public string StateOfResidencyName { get; set; }

		public CustomFieldModel CustomField { get; set; }

		public int? AccountLength { get; set; }

		public int? ContactLength { get; set; }

		[DisplayName("Display Name")]
		public string Alias { get; set; }

		public List<SelectListItem> DomesticForeigns { get; set; }

		public List<SelectListItem> InvestorEntityTypes { get; set; }

		public List<SelectListItem> Sources { get; set; }
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
			Country = (int)DefaultCountry.USA;
			CountryName = "United States";
		}

		public int? AddressId { get; set; }

		public int? ContactAddressId { get; set; }

		[DisplayName("Telephone No")]
		public string Phone { get; set; }

		[DisplayName("Fax No")]
		public string Fax { get; set; }

		[DisplayName("Email")]
		[Email(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }

		[DisplayName("Web Address")]
		[WebAddress(ErrorMessage = "Invalid Web Address")]
		public string WebAddress { get; set; }

		[Required(ErrorMessage = "Address1 is required")]
		[StringLength(40, ErrorMessage = "Address1 must be under 40 characters.")]
		[DisplayName("Address1")]
		public string Address1 { get; set; }

		[DisplayName("Address2")]
		public string Address2 { get; set; }

		[Required(ErrorMessage = "City is required")]
		[StringLength(30, ErrorMessage = "City must be under 30 characters.")]
		public string City { get; set; }

		[Required(ErrorMessage = "State is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "State is required")]
		[DisplayName("State")]
		public int? State { get; set; }

		[Required(ErrorMessage = "Investor is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Investor is required")]
		public int InvestorId { get; set; }

		public string StateName { get; set; }

		[DisplayName("Zip")]
		[Zip(ErrorMessage = "Invalid Zip")]
		public string Zip { get; set; }

		[DisplayName("Country")]
		[Required(ErrorMessage = "Country is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Country is required")]
		public int Country { get; set; }

		public string CountryName { get; set; }

		public object InvestorCommunications { get; set; }
	}

	public class ContactInformation : AddressInformation {
		public ContactInformation() {
			ContactId = 0;
			InvestorContactId = 0;
		}

		public int? ContactId { get; set; }

		public int? InvestorContactId { get; set; }

		[DisplayName("Contact Person")]
		public string Person { get; set; }

		[DisplayName("Designation")]
		public string Designation { get; set; }

		[DisplayName("Receives Distribution/Capital Call Notices")]
		public bool DistributionNotices { get; set; }

		[DisplayName("Financials")]
		public bool Financials { get; set; }

		[DisplayName("K1")]
		public bool K1 { get; set; }

		public IEnumerable<AddressInformation> AddressInformations { get; set; }

		public IEnumerable<ContactCommunicationInformation> ContactCommunications { get; set; }

		[DisplayName("Investor Letters")]
		public bool InvestorLetters { get; set; }
	}



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

	public class FundInformation {
		public FundInformation() {
			FundName = string.Empty;
			TotalCommitment = 0;
			UnfundedAmount = 0;
			InvestorType = string.Empty;
			FundClose = string.Empty;
		}

		public string FundName { get; set; }

		public decimal? TotalCommitment { get; set; }

		public decimal? UnfundedAmount { get; set; }

		public string InvestorType { get; set; }

		public string FundClose { get; set; }

		public int FundId { get; set; }

		public int? FundClosingId { get; set; }

		public int? InvestorTypeId { get; set; }

		public int InvestorFundTransactionId { get; set; }

		public int InvestorFundId { get; set; }

		public int InvestorId { get; set; }
	}

	public class ContactCommunicationInformation {

		public int? ContactCommunicationId { get; set; }

		public int CommunicationTypeId { get; set; }

		public string CommunicationValue { get; set; }

	}
}