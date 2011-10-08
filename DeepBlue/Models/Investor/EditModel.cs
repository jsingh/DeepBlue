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

}