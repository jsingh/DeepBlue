using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Report {
	public class FeesExpenseReportDetail {

		public DateTime? Date { get; set; }

		public string Type { get; set; }

		public decimal? Amount { get; set; }

		public string Note { get; set; }
	}
}