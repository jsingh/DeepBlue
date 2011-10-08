using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Investor {

	public class FundInformation {
		public FundInformation() {
			FundName = string.Empty;
			TotalCommitment = 0;
			UnfundedAmount = 0;
			InvestorType = string.Empty;
			FundClose = string.Empty;
		}

		public string FundName { get; set; }

		public decimal? TotalCommitment { get; set; }

		public decimal? UnfundedAmount { get; set; }

		public string InvestorType { get; set; }

		public string FundClose { get; set; }

		public int FundId { get; set; }

		public int? FundClosingId { get; set; }

		public int? InvestorTypeId { get; set; }

		public int InvestorFundTransactionId { get; set; }

		public int InvestorFundId { get; set; }

		public int InvestorId { get; set; }
	}
}