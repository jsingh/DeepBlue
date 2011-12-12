using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {

	public class UnderlyingDirectDividendDistributionModel   {

		public UnderlyingDirectDividendDistributionModel() {
			UnderlyingDirectDividendDistributionId = 0;
		}

		public int UnderlyingDirectDividendDistributionId { get; set; }
	 
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DisplayName("Paid Date")]
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

		public bool IsManualDividendDistribution { get; set; }

		[DisplayName("Post Record Date Transaction")]
		public bool? IsPostRecordDateTransaction { get; set; }

		[Required(ErrorMessage = "Amount is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Amount is required")]
		[DisplayName("Amount")]
		public decimal? Amount { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		[DisplayName("Fund")]
		public int FundId { get; set; }

		[Required(ErrorMessage = "Direct is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Direct is required")]
		[DisplayName("Fund")]
		public int SecurityID { get; set; }

		[Required(ErrorMessage = "Direct type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Direct type is required")]
		[DisplayName("Fund")]
		public int SecurityTypeID { get; set; }

		[Required(ErrorMessage = "Distribution Date is required")]
		[DateRange()]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DisplayName("Due Date")]
		public DateTime? DistributionDate { get; set; }

		[Required(ErrorMessage = "Received Date is required")]
		[DateRange()]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DisplayName("Received Date")]
		public DateTime? ReceivedDate { get; set; }

		public string FundName { get; set; }

		public IEnumerable<ActivityDealModel> Deals { get; set; }

	}
}