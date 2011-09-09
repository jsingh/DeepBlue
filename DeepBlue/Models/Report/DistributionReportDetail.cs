using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Report {
	public class DistributionReportDetail {

		public int DealNo { get; set; }

		public string FundName { get; set; }

		public DateTime? Date { get; set; }

		public decimal? Amount { get; set; }

		public string Type { get; set; }

		public string Stock { get; set; }

		public decimal? NoOfShares { get; set; }
	}
}