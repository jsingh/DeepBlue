using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {
	public class CapitalCallDetail {

		public string FundName { get; set; }

		public string CapitalCommitted { get; set; }

		public string UnfundedAmount { get; set; }

		public string ManagementFees { get; set; }

		public string FundExpenses { get; set; }
	}
}