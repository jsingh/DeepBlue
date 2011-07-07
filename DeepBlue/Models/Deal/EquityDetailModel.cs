using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace DeepBlue.Models.Deal {
	public class EquityDetailModel : EquityDocumentModel {

		public int EquityId { get; set; }

		[Required(ErrorMessage = "Issuer is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Issuer is required")]
		public int IssuerId { get; set; }

		[Required(ErrorMessage = "Equity Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Equity Type is required")]
		[DisplayName("Equity Type")]
		public int EquityTypeId { get; set; }

		[StringLength(50, ErrorMessage = "Symbol must be under 50 characters.")]
		[DisplayName("Stock Symbol")]
		public string Symbol { get; set; }

		[DisplayName("Is Public")]
		public bool Public { get; set; }

		[DisplayName("ISIN No./Cousip")]
		public int? ISINO { get; set; }

		[DisplayName("Seniority")]
		public string Seniority { get; set; }

		[DisplayName("Share Class")]
		public int? ShareClassTypeId { get; set; }

		[DisplayName("Industry")]
		public int? IndustryId { get; set; }

		[DisplayName("Currency")]
		public int? CurrencyId { get; set; }

		public string EquityType { get; set; }

		public string Currency { get; set; }

		public string Industry { get; set; }

		public string ShareClassType { get; set; }

		public List<SelectListItem> DocumentTypes { get; set; }

		public List<SelectListItem> UploadTypes { get; set; }

		public List<SelectListItem> Currencies { get; set; }

		public List<SelectListItem> EquityTypes { get; set; }

		public List<SelectListItem> ShareClassTypes { get; set; }

	}
}