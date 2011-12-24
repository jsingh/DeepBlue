using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Report {
	public class DealDetailReportModel {

		public bool IsTemplateDisplay { get; set; }

		public string FundName { get; set; }

		public int FundId { get; set; }

		public int DealNumber { get; set; }

		public string DealName { get; set; }

		public string Contact { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime? FirstFundCloseDate { get; set; }

		public decimal? NetPurchasePrice { get; set; }

		public decimal? ClosingCosts { get; set; }

		public decimal? TotalInitialInvestment {
			get {
				return (this.NetPurchasePrice + this.ClosingCosts);
			}
		}

		public decimal? AMBContributions { get; set; }

		public decimal? TotalCashOut {
			get {
				// SUM (UF.(CommitedAmount - UnfundedAmount)) + SUM (Direct.NumberOfShares * PurchasePrice) for all UnderlyingFunds and Directs in the deal
				return (this.OriginalCommitment - this.UnfundedClosing) + (this.NoOfShares * this.PurchasePrice);
			}
		}

		public decimal? CurrentUnfunded {
			get {
				return (this.OrginalUnfunded ?? 0) - (this.TotalCapitalCall ?? 0);
			}
		}

		public decimal? OrginalUnfunded { get; set; }

		// Sum of all capital calls since deal close
		public decimal? TotalCapitalCall { get; set; }

		public decimal? OriginalCommitment { get; set; }

		public decimal? UnfundedClosing { get; set; }

		public decimal? UnfundedClosingPercentage {
			get {
				// SUM(UnderlyingFund.UnfundedAmount) for all underlying funds in the deal / SUM( UnderlyingFund.CommitmentAmount))
				return (this.UnfundedClosing) / (this.OriginalCommitment);
	 		}
		}

		public decimal? NoOfShares { get; set; }

		public decimal? PurchasePrice { get; set; }

		public decimal? OrginalDiscount { get; set; }

		public decimal? ReceivedDistributions {
			get {
				return decimal.Add((this.CashDistributions ?? 0), (this.StockDistributions ?? 0));
			}
		}

		public decimal? CashDistributions { get; set; }

		public decimal? StockDistributions { get; set; }

		public decimal? RealizedROI {
			get {
				// Distribution Received / Total Cash Out
				return (this.ReceivedDistributions / this.TotalCashOut);
			}
		}

		public decimal? EstimatedNAV { get; set; }

		public decimal? DistEstimatedNAV {
			get {
				// SUM (UF.FundNAV) + SUM (Direct.FMV) for all UnderlyingFunds and Directs in the deal
				return (this.ReceivedDistributions + this.EstimatedNAV);
			}
		}

		public decimal? UnrealizedROI { get; set; }

		public List<DealUnderlyingFundDetailModel> Details { get; set; }

	}
	 
}