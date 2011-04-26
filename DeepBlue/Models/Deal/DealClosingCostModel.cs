using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class DealClosingCostModel {

		public int? DealClosingCostId { get; set; }
	
		public string Description { get; set; }

		[Required(ErrorMessage = "Description is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Description is required")]
		public int DealClosingCostTypeId { get; set; }

		[Required(ErrorMessage = "Deal is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
		public int DealId { get; set; }

		[Required(ErrorMessage = "Amount is required")]
		[Range((double)1, (double)decimal.MaxValue, ErrorMessage = "Amount is required")]
		public decimal Amount { get; set; }

		[Required(ErrorMessage = "Date is required")]
		[DateRange(ErrorMessage = "Date is required")]
		public DateTime Date { get; set; }
	}
}