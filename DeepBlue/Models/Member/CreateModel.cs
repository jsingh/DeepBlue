using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace DeepBlue.Models.Member {
	public class CreateModel {
		public SelectListModel SelectList = new SelectListModel();

		[Required(ErrorMessage = "Required")]
		[DisplayName("Name:")]
		public string MemberName { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("Alias:")]
		public string Alias { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("Social Security/Tax Id:")]
		public int SocialSecurityTaxId { get; set; }

		[DisplayName("Domestic/Foreign:")]
		public bool DomesticForeign { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("State of residency:")]
		public int StateOfResidency { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("Entity Type:")]
		public int EntityType { get; set; }

		[DisplayName("Notes:")]
		public string Notes { get; set; }
						
		/* Address Information */
		[Required(ErrorMessage = "Required")]
		[DisplayName("Address Type:")]
		public int AddressType { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("Telephone No:")]
		public string Phone { get; set; }

		[DisplayName("Fax No:")]
		public string Fax { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("Email:")]
		public string Email { get; set; }

		[DisplayName("Web Aaddress:")]
		public string WebAddress { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("ADDRESS1:")]
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
		[DisplayName("Zip:")]
		public string Zip { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("Country:")]
		public int Country { get; set; }
		
		/* Bank Information */

		[DisplayName("Bank Name:")]
		public string BankName { get; set; }

		[DisplayName("A/C #:")]
		public string AccountNumber { get; set; }

		[DisplayName("ABA #:")]
		public string ABANumber { get; set; }

		[DisplayName("Account Of:")]
		public string AccountOf { get; set; }

		[DisplayName("Reference:")]
		public string Reference { get; set; }

		[DisplayName("Attention:")]
		public string Attention { get; set; }

		public int AccountLength { get; set; }

		/* Contact Information */
		public int ContactLength { get; set; }

		[Required(ErrorMessage = "Required")]
		[DisplayName("Contact Address Type:")]
		public int ContactAddressType { get; set; }

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

		[DisplayName("Distribution Notices:")]
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
		public List<SelectListItem> MemberEntityTypes { get; set; }
		public List<SelectListItem> AddressTypes { get; set; }
		public List<SelectListItem> DomesticForeigns { get; set; }
	}
}