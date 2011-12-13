using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.ComponentModel;

namespace DeepBlue.Models.Deal {
	public class PostRecordDividendDistributionModel {

		public int DividendDistributionID { get; set; }

		[Required(ErrorMessage = "Direct is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Direct is required")]
		public int SecurityID { get; set; }

		[Required(ErrorMessage = "Direct type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Direct type is required")]
		public int SecurityTypeID { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		public string FundName { get; set; }

		[Required(ErrorMessage = "Deal is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
		public int DealId { get; set; }

		public string DealName { get; set; }

		[Required(ErrorMessage = "Distribution Amount is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Distribution Amount is required")]
		public decimal? Amount { get; set; }

		[Required(ErrorMessage = "Distribution Date is required")]
		[DateRange()]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime? DistributionDate { get; set; }

	}
}