using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {
	public class CreateReceiveModel {

		[DisplayName("Fund")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		[DisplayName("Fund Name")]
		public string FundName { get; set; }

		[DisplayName("Capital Call#")]
		public string CapitalCallNumber { get; set; }

		public int CapitalCallId { get; set; }

		[DisplayName("Capital Call Amount")]
		[Required(ErrorMessage = "Capital Call Amount is required")]
		[Range(typeof(decimal),"1", "79228162514264337593543950335", ErrorMessage = "Capital Call Amount is required")]
		public decimal CapitalAmountCalled { get; set; }

		[DisplayName("Capital Call Date")]
		[Required(ErrorMessage = "Capital Call Date is required")]
        [DateRange()]
		public DateTime CapitalCallDate { get; set; }

		[DisplayName("Capital Call Due Date")]
		[Required(ErrorMessage = "Capital Call Due Date is required")]
        [DateRange()]
		public DateTime CapitalCallDueDate { get; set; }

		public List<CapitalCallLineItemDetail> Items { get; set; }

		public List<SelectListItem> CapitalCalls { get; set; }

		public int ItemCount { get; set; }
	}
 

	public class CapitalCallLineItemDetail {

		public int Index { get; set; }

		public string InvestorName { get; set; }

		public int CapitalCallLineItemId { get; set; }

		public decimal CapitalAmountCalled { get; set; }

		public decimal ManagementFees { get; set; }

		public decimal InvestmentAmount { get; set; }

		public decimal ManagementFeeInterest { get; set; }

		public decimal InvestedAmountInterest { get; set; }

		public bool Received { get; set; }

		public string ReceivedDate { get; set; }
	}
}