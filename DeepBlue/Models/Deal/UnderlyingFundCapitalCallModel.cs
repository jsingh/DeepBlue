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

		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "ManagementFee is required")]
		public decimal? ManagementFee { get; set; }

		[DisplayName("Deemed Capital Call")]
		public bool? IsDeemedCapitalCall { get; set; }

		public bool IsManualCapitalCall { get; set; }

		public IEnumerable<ActivityDealModel> Deals { get; set; }
	}

}