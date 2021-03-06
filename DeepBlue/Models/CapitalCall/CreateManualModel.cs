﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {
	public class CreateManualModel {

		[DisplayName("Fund")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		[DisplayName("Capital Call Amount")]
		[Required(ErrorMessage = "Capital Call Amount is required")]
		[Range(typeof(decimal),"1", "79228162514264337593543950335", ErrorMessage = "Capital Call Amount is required")]
		public decimal CapitalAmountCalled { get; set; }

		[DisplayName("Capital Call #")]
		public string CapitalCallNumber { get; set; }

		[DisplayName("Capital Call Date")]
		[Required(ErrorMessage = "Capital Call Date is required")]
        [DateRange()]
		public DateTime CapitalCallDate { get; set; }

		[DisplayName("Capital Call Due Date")]
		[Required(ErrorMessage = "Capital Call Due Date is required")]
        [DateRange()]
		public DateTime CapitalCallDueDate { get; set; }

		[DisplayName("New Investment Amount")]
		public decimal? NewInvestmentAmount { get; set; }

		[DisplayName("Existing Investment Amount")]
		public decimal? ExistingInvestmentAmount { get; set; }

		[DisplayName("Management Fees")]
		public decimal? ManagementFees { get; set; }

		[DisplayName("Management Fees Interest")]
		public decimal? ManagementFeeInterest { get; set; }

		[DisplayName("Invested Amount")]
		public decimal? InvestedAmount { get; set; }

		[DisplayName("Invested Amount Interest")]
		public decimal? InvestedAmountInterest { get; set; }

		[DisplayName("Fund Expense Amount")]
		public decimal? FundExpenses { get; set; }

		public int? InvestorCount { get; set; }
		
	}
}