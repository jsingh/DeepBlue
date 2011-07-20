using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {

	public class CapitalDistributionInvestorDetail {

		public string InvestorName { get; set; }

		public decimal? CapitalDistributed { get; set; }

		public decimal? ReturnManagementFees { get; set; }

		public decimal? ReturnFundExpenses { get; set; }

		public DateTime CapitalDistributionDate { get; set; }

		public DateTime CapitalDistributionDueDate { get; set; }

		public decimal? Profit { get; set; }

		public decimal? ProfitReturn { get; set; }
	}
}