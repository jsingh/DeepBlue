using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace DeepBlue.Models.Deal {
	public class UnderlyingFundManualCapitalCallModel : UnderlyingFundActivityFields {

		public UnderlyingFundManualCapitalCallModel() {
			UnderlyingFundCapitalCallId = 0;
		}

		public int UnderlyingFundCapitalCallId { get; set; }

		[DisplayName("Deemed Capital Call:")]
		public bool? IsDeemedCapitalCall { get; set; }

		public IEnumerable<CapitalCallDealDetailModel> Deals { get; set; }
	}

	public class CapitalCallDealDetailModel {

		public int DealId { get; set; }

		public int DealNumber { get; set; }

		public string DealName { get; set; }

		public decimal? CommitmentAmount { get; set; }

		public decimal? CallAmount { get; set; }
	}
}