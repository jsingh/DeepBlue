using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Report {
	public class FundBreakDownReportDetail {

		public string FundName { get; set; }

		public int? TotalUnderlyingFunds { get; set; }

		public int? TotalDirects { get; set; }

		public decimal? AvgDealSize { get; set; }

		public decimal? AvgFundAgeAtPurchase { get; set; }

		public decimal? Venture { get; set; }

		public decimal? Buyout { get; set; }

		public decimal? Mezzanine { get; set; }

		public decimal? FundOfFunds { get; set; }

		public decimal? BuyoutVenture { get; set; }

		public decimal? Partnered { get; set; }

		public bool IsTemplateDisplay { get; set; }
	}
}