using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Report {
	public class DistributionLineItem {

		public string InvestorName { get; set; }

		public string Designation { get; set; }

		public decimal? Commitment { get; set; }

		public decimal? DistributionAmount { get; set; }
	}
}