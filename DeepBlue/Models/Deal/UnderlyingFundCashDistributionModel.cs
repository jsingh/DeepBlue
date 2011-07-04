using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {

	public class UnderlyingFundCashDistributionModel : UnderlyingFundActivityFields {

		public UnderlyingFundCashDistributionModel() {
			UnderlyingFundCashDistributionId = 0;
			CashDistributionTypeId = 0;
		}

		public int UnderlyingFundCashDistributionId { get; set; }

		[Required(ErrorMessage = "Cash Distribution Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Cash Distribution Type is required")]
		[DisplayName("Cash Distribution:")]
		public int? CashDistributionTypeId { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DisplayName("Paid Date:")]
		public DateTime? PaidDate { get; set; }

		public decimal? TotalCommitmentAmount {
			get {
				decimal? totalCommitmentAmount = 0;
				if (this.Deals != null) {
					totalCommitmentAmount = Deals.Sum(deal => deal.CommitmentAmount);
				}
				return totalCommitmentAmount;
			}
		}
		
		public string CashDistributionType { get; set; }

		public List<SelectListItem> CashDistributionTypes { get; set; }

		public bool IsManualCashDistribution { get; set; }

		public IEnumerable<ActivityDealModel> Deals { get; set; }

	}

	 
}