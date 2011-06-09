using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class UnderlyingFundPostRecordCashDistributionModel {

		public UnderlyingFundPostRecordCashDistributionModel() {
			CashDistributionId = 0;
			UnderlyingFundId = 0;
			DealId = 0;
		}

		public int CashDistributionId { get; set; }

		[Required(ErrorMessage = "Underlying Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Underlying Fund is required")]
		public int UnderlyingFundId { get; set; }

		[Required(ErrorMessage = "Deal is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
		public int DealId { get; set; }

		[Required(ErrorMessage = "Distribution Amount is required")]
		[Range(typeof(decimal),"1", "79228162514264337593543950335", ErrorMessage = "Distribution Amount is required")]
		public decimal? Amount { get; set; }

		[Required(ErrorMessage = "Distribution Date is required")]
		[DateRange()]
		public DateTime? DistributionDate { get; set; }

		public string DealName { get; set; }

		public string FundName { get; set; }

	}
}