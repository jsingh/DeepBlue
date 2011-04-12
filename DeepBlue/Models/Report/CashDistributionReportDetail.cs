using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Report {
	public class CashDistributionReportDetail {

		public string FundName { get; set; }

		public string DistributionDate { get; set; }

		public string TotalDistributionAmount { get; set; }

		public string WithCarryAmount { get; set; }

		public string RepayManFees { get; set; }

		public List<DistributionLineItem> Items { get; set; }
	}
}