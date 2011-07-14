using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class DealClosingCostModel {

		public DealClosingCostModel() {
			DealClosingCostId = 0;
			Description = string.Empty;
			DealClosingCostTypeId = 0;
			DealId = 0;
			Amount = 0;
			Date = DateTime.MinValue;
		}

		public int? DealClosingCostId { get; set; }
	
		public string Description { get; set; }

		[Required(ErrorMessage = "Description is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Description is required")]
		public int DealClosingCostTypeId { get; set; }

		[Required(ErrorMessage = "Deal is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
		public int DealId { get; set; }

		[Required(ErrorMessage = "Amount is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Amount is required")]
		public decimal Amount { get; set; }

		[Required(ErrorMessage = "Date is required")]
		[DateRange(ErrorMessage = "Date is required")]
		public DateTime? Date { get; set; }
	}
}