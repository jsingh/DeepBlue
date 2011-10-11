using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Admin.Enums;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Investor {

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
		
		[Required(ErrorMessage = "Country is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Country is required")]
		[DisplayName("Country")]
		public int Country { get; set; }

		public string CountryName { get; set; }

		public object InvestorCommunications { get; set; }
	}

}