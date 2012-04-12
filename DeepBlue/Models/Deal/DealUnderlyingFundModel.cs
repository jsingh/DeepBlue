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
			RecordDate = DateTime.Now;
			CommittedAmount = 0;
		}

		public int? DealUnderlyingFundId { get; set; }

		public string FundName { get; set; }

		[Required(ErrorMessage = "Deal is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
		public int DealId { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		[Required(ErrorMessage = "Fund Name is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund Name is required")]
		public int UnderlyingFundId { get; set; }

		[Required(ErrorMessage = "Record Date is required")]
		[DateRange()]
		public DateTime? RecordDate { get; set; }

		[Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Fund NAV is required")]
		public decimal? FundNAV { get; set; }

		[Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Committed Amount is required")]
		public decimal? CommittedAmount { get; set; }

		public decimal? Percent { get; set; }

		public decimal? UnfundedAmount { get; set; }

		public decimal? GrossPurchasePrice { get; set; }

		public decimal? PostRecordDateCapitalCall { get; set; }

		public decimal? PostRecordDateDistribution { get; set; }

		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal ClosingId is required")]
		public int? DealClosingId { get; set; }

		public bool IsClose { get; set; }

		public decimal? NetPurchasePrice { get; set; }

		public DateTime? EffectiveDate { get; set; }

		#region Final Deal Close Properties

		public decimal? ReassignedGPP { get; set; }

		public decimal? AdjustedCost { get; set; }

		#endregion
	}
}