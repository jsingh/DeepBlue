using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Report {
	public class CapitalCallReportDetail {

		public string FundName { get; set; }

		public DateTime? CapitalCallDueDate { get; set; }

		public decimal? TotalCapitalCall { get; set; }

		public decimal? TotalManagementFees { get; set; }

		public decimal? TotalExpenses { get; set; }

		public decimal? AmountForInv { get; set; }

		public decimal? NewInv { get; set; }

		public decimal? ExistingInv { get; set; }

		public bool IsTemplateDisplay { get; set; }

		public IEnumerable<CapitalCallItem> Items { get; set; }
	}
}