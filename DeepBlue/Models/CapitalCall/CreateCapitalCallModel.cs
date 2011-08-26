using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {
	public class CreateCapitalCallModel {

		[DisplayName("Fund")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		[DisplayName("Capital Amount")]
		[Required(ErrorMessage = "Capital Amount Called is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "CapitalAmountCalled is required")]
		public decimal CapitalAmountCalled { get; set; }

		public string CapitalCallNumber { get; set; }

		[DisplayName("Capital Call Date")]
		[Required(ErrorMessage = "Capital Call Date is required")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DateRange()]
		public DateTime CapitalCallDate { get; set; }

		[DisplayName("Capital Call Due Date")]
		[Required(ErrorMessage = "Capital Call Due Date is required")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DateRange()]
		public DateTime CapitalCallDueDate { get; set; }

		[DisplayName("New Investment Amount")]
		public decimal? NewInvestmentAmount { get; set; }

		[DisplayName("Existing Investment Amount")]
		public decimal? ExistingInvestmentAmount { get; set; }

		[DisplayName("Management Fees")]
		public decimal? ManagementFees { get; set; }

		[DisplayName("Fund Expense Amount")]
		public decimal? FundExpenseAmount { get; set; }

		[DisplayName("FromDate")]
		public DateTime? FromDate { get; set; }

		[DisplayName("To")]
		public DateTime? ToDate { get; set; }

		[DisplayName("Management Fees Interest")]
		public decimal? ManagementFeeInterest { get; set; }

		[DisplayName("Invested Amount Interest")]
		public decimal? InvestedAmountInterest { get; set; }

		[DisplayName("Fund Expense Amount")]
		public decimal? FundExpenses { get; set; }

		[DisplayName("Invested Amount")]
		public decimal? InvestedAmount { get; set; }

		public int? InvestorCount { get; set; }

		public bool? AddManagementFees { get; set; }

		public bool? AddFundExpenses { get; set; }

	}
}