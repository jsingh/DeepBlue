using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Report {
	public class DealUnderlyingFundDetailModel {

		public DateTime? Date { get; set; }

		public string FundName { get; set; }

		public decimal? Amount { get; set; }

		public decimal? UnUsedCapital { get; set; }

		public string Type { get; set; }
	}
}