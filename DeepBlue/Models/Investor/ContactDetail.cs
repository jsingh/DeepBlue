using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Investor {
	public class ContactDetail {

		public ContactDetail() {
			ContactCountry = (int)DeepBlue.Models.Admin.Enums.DefaultCountry.USA;
			ContactCountryName = "United States";
		}

		[DisplayName("Contact Person")]
		public string ContactPerson { get; set; }

		[DisplayName("Designation")]
		public string Designation { get; set; }

		[DisplayName("Telephone No")]
		public string ContactPhoneNumber { get; set; }

		[DisplayName("Fax No")]
		public string ContactFaxNumber { get; set; }

		[DisplayName("Email")]
		public string ContactEmail { get; set; }

		[DisplayName("Web Address")]
		public string ContactWebAddress { get; set; }

		[DisplayName("Address1")]
		public string ContactAddress1 { get; set; }

		[DisplayName("Address2")]
		public string ContactAddress2 { get; set; }

		[DisplayName("City")]
		public string ContactCity { get; set; }

		[DisplayName("State")]
		public int ContactState { get; set; }

		public string ContactStateName { get; set; }

		[DisplayName("Zip")]
		public string ContactZip { get; set; }

		[DisplayName("Country")]
		public int ContactCountry { get; set; }

		public string ContactCountryName { get; set; }

		[DisplayName("Receives Distribution/Capital Call Notices")]
		public bool DistributionNotices { get; set; }

		[DisplayName("Financials")]
		public bool Financials { get; set; }

		[DisplayName("K1")]
		public bool K1 { get; set; }

		[DisplayName("Investor Letters")]
		public bool InvestorLetters { get; set; }

		

	}
}