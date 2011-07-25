using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.ComponentModel;
using System.Web.Mvc;

namespace DeepBlue.Models.Deal {
	public class IssuerDetailModel {

		public IssuerDetailModel() {
			CountryId = (int)DeepBlue.Models.Admin.Enums.DefaultCountry.USA;
			Country = "United States";
		}
		
		public int IssuerId { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		[StringLength(100, ErrorMessage = "Name must be under 100 characters.")]
		[DisplayName("Issuer Name")]
		public string Name { get; set; }

		[StringLength(100, ErrorMessage = "Parent Name must be under 100 characters.")]
		[DisplayName("Parent Name")]
		public string ParentName { get; set; }

		[Required(ErrorMessage = "Country is required.")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Country is required.")]
		[DisplayName("Country")]
		public int? CountryId { get; set; }

		public string Country { get; set; }

		public int? IssuerRatingId { get; set; }

		public List<SelectListItem> IssuerRatings { get; set; }

		public bool IsUnderlyingFundModel { get; set; }
	}
}