using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Report {
	public class SecurityValueReportDetail {

		public int DealNo { get; set; }

		public string Security { get; set; }

		public string SecurityType { get; set; }

		public decimal? NoOfShares { get; set; }

		public decimal? Price { get; set; }

		public decimal? Value { get; set; }

		public DateTime? Date { get; set; }
	 
	}
}