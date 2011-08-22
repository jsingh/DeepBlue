using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Report {
	public class CashDistributionReportDetail {

		public string FundName { get; set; }

		public DateTime DistributionDate { get; set; }

		public decimal TotalDistributionAmount { get; set; }

		public decimal WithCarryAmount { get; set; }

		public decimal RepayManFees { get; set; }

		public bool IsTemplateDisplay { get; set; }

		public IEnumerable<DistributionLineItem> Items { get; set; }
	}
}