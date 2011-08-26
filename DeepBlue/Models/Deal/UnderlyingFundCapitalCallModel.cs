using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DeepBlue.Models.Deal {
	public class UnderlyingFundCapitalCallModel : UnderlyingFundActivityFields {

		public UnderlyingFundCapitalCallModel() {
			UnderlyingFundCapitalCallId = 0;
		}

		public int UnderlyingFundCapitalCallId { get; set; }

		public decimal? TotalCommitmentAmount {
			get {
				decimal? totalCommitmentAmount = 0;
				if (this.Deals != null) {
					totalCommitmentAmount = Deals.Sum(deal => deal.CommitmentAmount);
				}
				return totalCommitmentAmount;
			}
		}

		[DisplayName("Deemed Capital Call")]
		public bool? IsDeemedCapitalCall { get; set; }

		public bool IsManualCapitalCall { get; set; }

		public IEnumerable<ActivityDealModel> Deals { get; set; }
	}

}