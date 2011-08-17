using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace DeepBlue.Models.Deal {
	public class EquityDetailModel : EquityDocumentModel {

		public EquityDetailModel() {
			EquityCurrencyId = (int)DeepBlue.Models.Deal.Enums.Currency.USD;
		}

		public int EquityId { get; set; }

		[Required(ErrorMessage = "Company is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Company is required")]
		public int IssuerId { get; set; }

		[Required(ErrorMessage = "Equity Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Equity Type is required")]
		[DisplayName("Equity Type")]
		public int EquityTypeId { get; set; }

		[StringLength(50, ErrorMessage = "Symbol must be under 50 characters.")]
		[DisplayName("Stock Symbol")]
		public string EquitySymbol { get; set; }

		[DisplayName("Security Type")]
		public int EquitySecurityTypeId { get; set; }

		[DisplayName("CUSIP NO")]
		[StringLength(50, ErrorMessage = "SIN No./Cousip must be under 50 characters.")]
		public string EquityISINO { get; set; }

		[DisplayName("Share Class")]
		public int? ShareClassTypeId { get; set; }

		[DisplayName("Industry")]
		public int? EquityIndustryId { get; set; }

		[DisplayName("Currency")]
		public int? EquityCurrencyId { get; set; }

		[DisplayName("Comments")]
		[StringLength(105, ErrorMessage = "EquityComments must be under 105 characters.")]
		public string EquityComments { get; set; }

		public string EquityType { get; set; }

		public string EquityCurrency { get; set; }

		public string EquityIndustry { get; set; }

		public string ShareClassType { get; set; }

		public List<SelectListItem> DocumentTypes { get; set; }

		public List<SelectListItem> UploadTypes { get; set; }

		public List<SelectListItem> Currencies { get; set; }

		public List<SelectListItem> EquityTypes { get; set; }

		public List<SelectListItem> ShareClassTypes { get; set; }

		public List<SelectListItem> EquitySecurityTypes { get; set; }

	}
}