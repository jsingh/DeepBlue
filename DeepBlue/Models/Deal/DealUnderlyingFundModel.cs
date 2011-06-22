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
			Percent = 0;
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

		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Fund NAV is required")]
		public decimal? FundNAV { get; set; }
		
		public decimal? Percent { get; set; }

		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Committed Amount is required")]
		public decimal? CommittedAmount { get; set; }

		public decimal? UnfundedAmount { get; set; }

		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Gross Purchase Price is required")]
		public decimal? GrossPurchasePrice { get; set; }

		[Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Reassigned GPP is required")]
		public decimal? ReassignedGPP { get; set; }

		public decimal? PostRecordDateCapitalCall { get; set; }

		public decimal? PostRecordDateDistribution { get; set; }

		public decimal NetPurchasePrice {
			get {
				return ((this.GrossPurchasePrice ?? 0) + (this.PostRecordDateCapitalCall ?? 0) - (this.PostRecordDateDistribution ?? 0));
			}
		}

		public string FormatNPP { get { return FormatHelper.CurrencyFormat(this.NetPurchasePrice); } }

		public decimal AdjustedCost {
			get {
				return (this.ReassignedGPP ?? 0) + (this.PostRecordDateCapitalCall ?? 0) - (this.PostRecordDateDistribution ?? 0);
			}
		}

		public string FormatAJC { get { return FormatHelper.CurrencyFormat(this.AdjustedCost); } }

		public int? DealClosingId { get; set; }
	}
}