using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Deal {
	public class UnfundedAdjustmentModel {

		[Required(ErrorMessage = "Underlying Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Underlying Fund is required")]
		public int UnderlyingFundId { get; set; }

		[Required(ErrorMessage = "Deal Underlying Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal Underlying Fund is required")]
		public int DealUnderlyingFundId { get; set; }

		public int DealUnderlyingFundAdjustmentId { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		public string FundName { get; set; }

		[Required(ErrorMessage = "Commitment Amount is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Commitment Amount is required")]
		public decimal? CommitmentAmount { get; set; }

		[Required(ErrorMessage = "Unfunded Amount is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Unfunded Amount is required")]
		public decimal? UnfundedAmount { get; set; }

	}
}