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
		[StringLength(100, ErrorMessage = "Investor Name must be under 100 characters.")]
		[DisplayName("Investor Name")]
		public string InvestorName { get; set; }

		[DisplayName("Display Name")]
		public string Alias { get; set; }

		[Required(ErrorMessage = "Social Security/ Tax Id is required")]
		[StringLength(50, ErrorMessage = "Investor Name must be under 50 characters.")]
		[DisplayName("Social Security/ Tax Id")]
		public string SocialSecurityTaxId { get; set; }

		[DisplayName("Domestic/ Foreign")]
		public bool DomesticForeign { get; set; }

		[Required(ErrorMessage = "State Of Residency is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "State Of Residency is required")]
		[DisplayName("State of Residency")]
		public int StateOfResidency { get; set; }

		[Required(ErrorMessage = "EntityType is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "EntityType is required")]
		[DisplayName("EntityType")]
		public int EntityType { get; set; }

		[DisplayName("FOIA")]
		public bool FOIA { get; set; }

		[DisplayName("ERISA")]
		public bool ERISA { get; set; }

		[DisplayName("Restricted Person")]
		public bool RestrictedPerson { get; set; }

		[DisplayName("Source")]
		public int Source { get; set; }

		[DisplayName("Notes")]
		public string Notes { get; set; }

		/* Address Information */
		[DisplayName("Telephone No")]
		public string Phone { get; set; }

		[DisplayName("Fax No")]
		public string Fax { get; set; }

		[DisplayName("Email")]
		public string Email { get; set; }

		[DisplayName("Web Address")]
		public string WebAddress { get; set; }

		[DisplayName("Address1")]
		public string Address1 { get; set; }

		[DisplayName("Address2")]
		public string Address2 { get; set; }

		[DisplayName("City")]
		public string City { get; set; }

		[DisplayName("State")]
		public int State { get; set; }

		public string StateName { get; set; }

		[DisplayName("Zip")]
		public string Zip { get; set; }

		[DisplayName("Country")]
		public int Country { get; set; }

		public string CountryName { get; set; }

		public int AccountLength { get; set; }

		/* Contact Information */
		public int ContactLength { get; set; }

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