using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.CapitalCall {
	public class FundDetail {
		
		public int FundId { get; set; }

		public string FundName { get; set; }

		public decimal? TotalCommitment { get; set; }

		public decimal? UnfundedAmount { get; set; }

		public decimal? TotalDistribution { get; set; }

		public decimal? TotalProfit { get; set; }

		public int? CapitalCallNumber { get; set; }

		public int? DistributionNumber { get; set; }

	}
}