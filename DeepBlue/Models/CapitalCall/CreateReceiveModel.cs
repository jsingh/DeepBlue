using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using System.ComponentModel;
using System.Web.Mvc;

namespace DeepBlue.Models.CapitalCall {
	public class CreateReceiveModel {

		public int FundId { get; set; }

		[DisplayName("Fund Name:")]
		public string FundName { get; set; }

		[DisplayName("Capital Call#:")]
		public string CapitalCallNumber { get; set; }

		public int CapitalCallId { get; set; }

		[DisplayName("Capital Call Amount:")]
		public decimal CapitalAmountCalled { get; set; }

		[DisplayName("Capital Call Date:")]
		public DateTime CapitalCallDate { get; set; }

		[DisplayName("Capital Call Due Date:")]
		public DateTime CapitalCallDueDate { get; set; }

		public List<CapitalCallLineItemDetail> Items { get; set; }

		public List<SelectListItem> CapitalCalls { get; set; }

		public int ItemCount { get; set; }
	}
 

	public class CapitalCallLineItemDetail{

		public string InvestorName { get; set; }

		public int CapitalCallLineItemId { get; set; }

		public decimal CapitalAmountCalled { get; set; }

		public decimal ManagementFees { get; set; }

		public decimal InvestmentAmount { get; set; }

		public decimal ManagementFeeInterest { get; set; }

		public decimal InvestedAmountInterest { get; set; }

		public bool Received { get; set; }

		public DateTime ReceivedDate { get; set; }
	}
}