using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.CapitalCall.Enums;

namespace DeepBlue.Models.CapitalCall {
	public class DetailModel {

		public int FundId { get; set; }

		public string FundName { get; set; }

		public DetailType DetailType { get; set; }

		#region CapitalCall

		public decimal? CapitalCommitted { get; set; }

		public decimal? UnfundedAmount { get; set; }

		public decimal? ManagementFees { get; set; }

		public decimal? FundExpenses { get; set; }

		public IEnumerable<CapitalCallDetail> CapitalCalls { get; set; }

		#endregion

		#region CapitalDistribution

		public decimal? CapitalDistributed { get; set; }

		public decimal? ReturnManagementFees { get; set; }

		public decimal? ReturnFundExpenses { get; set; }

		public decimal? ProfitsReturned { get; set; }

		public IEnumerable<CapitalDistributionDetail> CapitalDistributions { get; set; }

		#endregion
	}
	 
}