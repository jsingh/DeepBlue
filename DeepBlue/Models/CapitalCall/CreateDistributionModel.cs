using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.CapitalCall {
	public class CreateDistributionModel {
		
		public int FundId { get; set; }

		public decimal DistributionAmount { get; set; }

		public DateTime DistributionDate { get; set; }

		public DateTime DistributionDueDate { get; set; }

		public bool AddPreferredAmount { get; set; }

		public bool AddReturnManagementFees { get; set; }

		public bool AddReturnFundExpenses { get; set; }

		public bool AddPreferredCatchUp { get; set; }

		public bool Profits { get; set; }

		public decimal? PreferredAmount { get; set; }

		public decimal? ReturnManagementFees { get; set; }

		public decimal? ReturnFundExpenses { get; set; }

		public decimal? PreferredCatchUp { get; set; }

		public decimal? GPProfits { get; set; }

		public decimal? LPProfits { get; set; }
	}
}