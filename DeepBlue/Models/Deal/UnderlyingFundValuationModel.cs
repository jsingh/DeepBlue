using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class UnderlyingFundValuationModel {

		public UnderlyingFundValuationModel() {
			UnderlyingFundId = 0;
			FundId = 0;
			UnderlyingFundNAVId = 0;
			UpdateDate = DateTime.Now;
			TotalCapitalCall = 0;
			TotalDistribution = 0;
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
				return (this.FundNAV ?? 0) + (this.TotalCapitalCall - this.TotalDistribution);
			}
		}

		public decimal? FundNAV { get; set; }

		public decimal TotalCapitalCall { get; set; }

		public decimal TotalDistribution { get; set; }

		public DateTime? FundNAVDate { get; set; }

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