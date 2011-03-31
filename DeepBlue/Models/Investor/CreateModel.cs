using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Investor {
	public class CreateModel {

		public SelectListModel SelectList = new SelectListModel();

		public int InvestorId { get; set; }

		[Required(ErrorMessage = "Investor Name is required")]
		[RemoteUID_(Action = "InvestorNameAvailable", Controller = "Investor", ValidateParameterName = "InvestorName", Params = new string[] { "InvestorId" })]
		[DisplayName("Investor Name:")]
		[StringLength(100, ErrorMessage = "Investor Name must be under 100 characters.")]
		public string InvestorName { get; set; }

		[DisplayName("Display Name:")]
		public string Alias { get; set; }

		[Required(ErrorMessage = "Social Security/ Tax Id is required")]
		[DisplayName("Social Security/ Tax Id:")]
		[StringLength(50, ErrorMessage = "Investor Name must be under 50 characters.")]
		public string SocialSecurityTaxId { get; set; }

		[DisplayName("Domestic/ Foreign:")]
		public bool DomesticForeign { get; set; }

		[Required(ErrorMessage = "State Of Residency is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "State Of Residency is required")]
		[DisplayName("State of Residency:")]
		public int StateOfResidency { get; set; }

		[Required(ErrorMessage = "EntityType is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "EntityType is required")]
		[DisplayName("EntityType:")]
		public int EntityType { get; set; }

		[DisplayName("FOIA:")]
		public bool FOIA { get; set; }

		[DisplayName("ERISA:")]
		public bool ERISA { get; set; }

		[DisplayName("Restricted Person:")]
		public bool RestrictedPerson { get; set; }

		[DisplayName("Source:")]
		public int Source { get; set; }

		[DisplayName("Notes:")]
		public string Notes { get; set; }

		/* Address Information */
		[DisplayName("Telephone No:")]
		public string Phone { get; set; }

		[DisplayName("Fax No:")]
		public string Fax { get; set; }

		[RegularExpression("^([_a-zA-Z0-9-]+)(\\.[_a-zA-Z0-9-]+)*@([a-zA-Z0-9-]+\\.)+([a-zA-Z]{2,3})$", ErrorMessage = "Invalid Email")]
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

		[RegularExpression("^(\\d{5}-\\d{4}|\\d{5}|\\d{9})$|^([a-zA-Z]\\d[a-zA-Z]\\d[a-zA-Z]\\d)$", ErrorMessage = "Invalid Zip")]
		[DisplayName("Zip:")]
		public string Zip { get; set; }

		[DisplayName("Country:")]
		public int Country { get; set; }

		/* Bank Information */

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

		public int AccountLength { get; set; }

		/* Contact Information */
		public int ContactLength { get; set; }

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

		public CustomFieldModel CustomField { get; set; }
	}

	public class SelectListModel {
		public List<SelectListItem> Countries { get; set; }
		public List<SelectListItem> States { get; set; }
		public List<SelectListItem> InvestorEntityTypes { get; set; }
		public List<SelectListItem> AddressTypes { get; set; }
		public List<SelectListItem> DomesticForeigns { get; set; }
		public List<SelectListItem> Source { get; set; }
	}
}