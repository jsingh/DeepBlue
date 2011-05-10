using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Issuer {
	public class EquityDetailModel {

		public int EquityId { get; set; }

		[Required(ErrorMessage = "Issuer is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Issuer is required")]
		public int IssuerId { get; set; }

		[Required(ErrorMessage = "Equity Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Equity Type is required")]
		public int EquityTypeId { get; set; }

		[Required(ErrorMessage = "Symbol is required")]
		[StringLength(50, ErrorMessage = "Symbol must be under 50 characters.")]
		public string Symbol { get; set; }

		public bool Public { get; set; }

		public int? ShareClassTypeId { get; set; }

		public int? IndustryId { get; set; }

		public int? CurrencyId { get; set; }

		public string EquityType { get; set; }

		public string Currency { get; set; }

		public string Industry { get; set; }

		public string ShareClassType { get; set; }

	}
}