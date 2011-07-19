using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {

	public class CapitalCallInvestorDetail {

		public string InvestorName { get; set; }

		public decimal? CapitalCallAmount { get; set; }

		public decimal? ManagementFees { get; set; }

		public decimal? FundExpenses { get; set; }

		public DateTime CapitalCallDate { get; set; }

		public DateTime CapitalCallDueDate { get; set; }
	}
}