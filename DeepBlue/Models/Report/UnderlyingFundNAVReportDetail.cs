using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Report {
	public class UnderlyingFundNAVReportDetail {

		public DateTime? Date { get; set; }

		public int? DealNo { get; set; }

		public string FundName { get; set; }

		public decimal? NAV { get; set; }

		public DateTime? Receipt { get; set; }

		public string Frequency { get; set; }

		public string Method { get; set; }
	}
}