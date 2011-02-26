using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace DeepBlue.Models.Investor {
	public class CreateModel {
		public SelectListModel SelectList = new SelectListModel();

		[Required(ErrorMessage = "Required")]
		[DisplayName("Investor Name:")]
		public string InvestorName { get; set; }
 
		[Required(ErrorMessage = "Required")]
		[DisplayName("Display Name:")]
		public string Alias { get; set; }

		[Required(ErrorMessage = "Required")]
		[RegularExpression("^\\d{3}-\\d{2}-\\d{4}$", ErrorMessage = "Invalid Social Security/ Tax Id")]
		[DisplayName("Social Security/ Tax Id:")]
		public string SocialSecurityTaxId { get; set; }

		[DisplayName("Domestic/ Foreign:")]
		public bool DomesticForeign { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("State of Residency:")]
		public int StateOfResidency { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("Entity Type:")]
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
		[Required(ErrorMessage = "Required")]
		[RegularExpression("^[01]?[-.]?(\\([2-9]\\d{2}\\)|[2-9]\\d{2})[-.]?\\d{3}[-.]?\\d{4}$", ErrorMessage = "Invalid Telephone No")]
		[DataType(DataType.PhoneNumber)]
		[DisplayName("Telephone No:")]
		public string Phone { get; set; }

		[DisplayName("Fax No:")]
		public string Fax { get; set; }

		[Required(ErrorMessage = "Required")]
		[RegularExpression("^([_a-zA-Z0-9-]+)(\\.[_a-zA-Z0-9-]+)*@([a-zA-Z0-9-]+\\.)+([a-zA-Z]{2,3})$",ErrorMessage="Invalid Email")]
		[DataType(DataType.EmailAddress)]
		[DisplayName("Email:")]
		public string Email { get; set; }

		[DataType(DataType.Url)]
		[DisplayName("Web Address:")]
		public string WebAddress { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("Address1:")]
		public string Address1 { get; set; }

		[DisplayName("Address2:")]
		public string Address2 { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("City:")]
		public string City { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("State:")]
		public int State { get; set; }

		[Required(ErrorMessage = "Required")]
		[RegularExpression("^(\\d{5}-\\d{4}|\\d{5}|\\d{9})$|^([a-zA-Z]\\d[a-zA-Z]\\d[a-zA-Z]\\d)$", ErrorMessage = "Invalid Zip")]
		[DisplayName("Zip:")]
		public string Zip { get; set; }

		[Required(ErrorMessage = "Required")]
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