using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Deal {
	public class UnderlyingFundAddressInformation {

		public UnderlyingFundAddressInformation() {
			Country = (int)DeepBlue.Models.Admin.Enums.DefaultCountry.USA;
			CountryName = "United States";
		}

		public int? UnderlyingFundId { get; set; }
 
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

		public string StateName { get; set; }

		[DisplayName("Zip")]
		[Zip(ErrorMessage = "Invalid Zip")]
		public string Zip { get; set; }

		[DisplayName("Country")]
		[Required(ErrorMessage = "Country is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Country is required")]
		public int Country { get; set; }

		public string CountryName { get; set; }

	}
}