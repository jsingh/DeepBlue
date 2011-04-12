using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Report {
	public class CapitalCallItem {

		public string InvestorName { get; set; }

		public decimal? Commitment { get; set; }

		public decimal? Investments { get; set; }

		public decimal? ManagementFees { get; set; }

		public decimal? Expenses { get; set; }

		public decimal Total {
			get {
				return (Investments + ManagementFees + Expenses) ?? 0;
			}
		}
	}
}