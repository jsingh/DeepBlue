using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace DeepBlue.Models.Deal {
	public class FixedIncomeDetailModel : FixedIncomeDocumentModel {

		public FixedIncomeDetailModel() {
			FixedIncomeCurrencyId = (int)DeepBlue.Models.Deal.Enums.Currency.USD;
		}

		public int FixedIncomeId { get; set; }

		[Required(ErrorMessage = "Company is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Company is required")]
		public int IssuerId { get; set; }

		[Required(ErrorMessage = "Fixed Income Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fixed Income Type is required")]
		[DisplayName("Fixed Income Type")]
		public int FixedIncomeTypeId { get; set; }

		[StringLength(50, ErrorMessage = "Symbol must be under 50 characters.")]
		[DisplayName("Security Name")]
		public string FixedIncomeSymbol { get; set; }

		[DisplayName("Face Value")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Face Value is required")]
		public decimal? FaceValue { get; set; }

		[DisplayName("Maturity")]
		public DateTime? Maturity { get; set; }

		[DisplayName("Issued Date")]
		public DateTime? IssuedDate { get; set; }

		[DisplayName("Currency")]
		public int? FixedIncomeCurrencyId { get; set; }

		[DisplayName("Frequency")]
		public int? Frequency { get; set; }

		[DisplayName("First Accrual Date")]
		public DateTime? FirstAccrualDate { get; set; }

		[DisplayName("First Coupon Date")]
		public DateTime? FirstCouponDate { get; set; }

		[DisplayName("Industry")]
		public int? FixedIncomeIndustryId { get; set; }

		[DisplayName("Industry")]
		public string FixedIncomeIndustry { get; set; }

		[DisplayName("Coupon Information")]
		public string CouponInformation { get; set; }
				
		public string FixedIncomeType { get; set; }

		[DisplayName("Identifier")]
		[StringLength(50, ErrorMessage = "SIN No./Cousip must be under 50 characters.")]
		public string FixedIncomeISINO { get; set; }

		[DisplayName("Comments")]
		[StringLength(105, ErrorMessage = "FixedIncomeComments must be under 105 characters.")]
		public string FixedIncomeComments { get; set; }

		public List<SelectListItem> Currencies { get; set; }

		public List<SelectListItem> FixedIncomeTypes { get; set; }

		public List<SelectListItem> DocumentTypes { get; set; }

		public List<SelectListItem> UploadTypes { get; set; }

	}
}