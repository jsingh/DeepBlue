using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Deal {
	public class FixedIncomeDetailModel {

		public int FixedIncomeId { get; set; }

		[Required(ErrorMessage = "Issuer is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Issuer is required")]
		public int IssuerId { get; set; }

		[Required(ErrorMessage = "Fixed Income Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fixed Income Type is required")]
		public int FixedIncomeTypeId { get; set; }

		[Required(ErrorMessage = "Symbol is required")]
		[StringLength(50, ErrorMessage = "Symbol must be under 50 characters.")]
		public string Symbol { get; set; }

		public decimal? FaceValue { get; set; }

		public DateTime? Maturity { get; set; }

		public DateTime? IssuedDate { get; set; }

		public int? CurrencyId { get; set; }

		public int? Frequency { get; set; }

		public DateTime? FirstAccrualDate { get; set; }

		public DateTime? FirstCouponDate { get; set; }

		public int? IndustryId { get; set; }

		public string CouponInformation { get; set; }

		public string FixedIncomeType { get; set; }

	}
}