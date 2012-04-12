using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity;

namespace DeepBlue.Models.Deal {
	public class UnderlyingFundValuationModel {

		public UnderlyingFundValuationModel() {
			UnderlyingFundId = 0;
			FundId = 0;
			UnderlyingFundNAVId = 0;
			UpdateDate = DateTime.Now;
			TotalCapitalCall = 0;
			TotalDistribution = 0;
			TotalPostRecordCapitalCall = 0;
			TotalPostRecordDistribution = 0;
		}

		public int UnderlyingFundNAVId { get; set; }

		[Required(ErrorMessage = "Underlying Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Underlying Fund is required")]
		public int UnderlyingFundId { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }
	 
		public decimal CalculateNAV {
			get {
				/*Calculate NAV = Total Fund NAV + 
					  			 ((Total Capital Call + Total Post Record Capital Call) – (Total Distribution + Total Post Record Distribution)).
				*/
				return (this.FundNAV ?? 0) + (((this.TotalCapitalCall ?? 0) + (this.TotalPostRecordCapitalCall ?? 0))
											  - ((this.TotalDistribution ?? 0) + (this.TotalPostRecordDistribution ?? 0)));
			}
		}

		public object Distributions { get; set; }

		public decimal? FundNAV { get; set; }

		public decimal? TotalCapitalCall { get; set; }

		public decimal? TotalDistribution { get; set; }

		public decimal? TotalPostRecordCapitalCall { get; set; }

		public decimal? TotalPostRecordDistribution { get; set; }

		public DateTime? FundNAVDate { get; set; }

		public DateTime? EffectiveDate { get; set; }

		[Required(ErrorMessage = "Update NAV is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Update NAV is required")]
		public decimal? UpdateNAV { get; set; }

		[Required(ErrorMessage = "Update Date is required")]
		[DateRange()]
		public DateTime UpdateDate { get; set; }

		public string UnderlyingFundName { get; set; }

		public string FundName { get; set; }

	}
}