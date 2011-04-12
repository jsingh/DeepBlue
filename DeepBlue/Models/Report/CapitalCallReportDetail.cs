using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Report {
	public class CapitalCallReportDetail {

		public string FundName { get; set; }

		public string CapitalCallDueDate { get; set; }

		public string TotalCapitalCall { get; set; }

		public string TotalManagementFees { get; set; }

		public string TotalExpenses { get; set; }

		public string AmountForInv { get; set; }

		public string NewInv { get; set; }

		public string ExistingInv { get; set; }

		public List<CapitalCallItem> Items { get; set; }
	}
}