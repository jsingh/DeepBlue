using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Deal {

	public class CreateActivityModel {

		public CreateActivityModel() {
			UnderlyingFundCapitalCallModel = new UnderlyingFundCapitalCallModel();
			UnderlyingFundCashDistributionModel = new UnderlyingFundCashDistributionModel();
		}

		public UnderlyingFundCapitalCallModel UnderlyingFundCapitalCallModel { get; set; }

		public UnderlyingFundCashDistributionModel UnderlyingFundCashDistributionModel { get; set; }

		public List<UnderlyingFundCapitalCallList> UnderlyingFundCapitalCalls { get; set; }

		public List<UnderlyingFundCashDistributionList> UnderlyingFundCashDistributions { get; set; }
	}
}