using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.Web.Mvc;

namespace DeepBlue.Models.Investor {
	public class InvestorInformation {

		[DisplayName("InvestorName")]
		[Required(ErrorMessage = "Investor Name is required")]
		[StringLength(100, ErrorMessage = "Investor Name must be under 30 characters.")]
		public string InvestorName { get; set; }

		[DisplayName("FOIA")]
		public bool FOIA { get; set; }

		[DisplayName("Source")]
		public string Source { get; set; }

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
}