using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class DealUnderlyingFundModel {

		public DealUnderlyingFundModel() {
			DealUnderlyingFundId = 0;
			FundName = string.Empty;
			DealId = 0;
			UnderlyingFundId = 0;
			RecordDate = Convert.ToDateTime("01/01/1900");
			FundNav = 0;
			Percent = 0;
			CommittedAmount = 0;
		}

		public int? DealUnderlyingFundId { get; set; }

		public string FundName { get; set; }

		[Required(ErrorMessage = "Deal is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
		public int DealId { get; set; }

		[Required(ErrorMessage = "Fund Name is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund Name is required")]
		public int UnderlyingFundId { get; set; }

		[Required(ErrorMessage = "Record Date is required")]
		[DateRange()]
		public DateTime RecordDate { get; set; }

		[Range((double)0, (double)decimal.MaxValue, ErrorMessage = "FundNav is required")]
		public decimal? FundNav { get; set; }

		[Range((double)0, (double)100, ErrorMessage = "Percent must be under 100%.")]
		public decimal? Percent { get; set; }

		[Range((double)0, (double)decimal.MaxValue, ErrorMessage = "Committed Amount is required")]
		public decimal? CommittedAmount { get; set; }
	}
}