using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {
	public class CreateCapitalCallModel {

		public int? CapitalCallID { get; set; }

		[DisplayName("Fund")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		[DisplayName("Capital Amount")]
		[Required(ErrorMessage = "Capital Amount Called is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "CapitalAmountCalled is required")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##;-0.##;\\}")]
		public decimal CapitalAmountCalled { get; set; }

		[DisplayName("Capital Call Number")]
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
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##;-0.##;\\}")]
		public decimal? NewInvestmentAmount { get; set; }

		[DisplayName("Existing Investment Amount")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##;-0.##;\\}")]
		public decimal? ExistingInvestmentAmount { get; set; }

		[DisplayName("Management Fees")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##;-0.##;\\}")]
		public decimal? ManagementFees { get; set; }

		[DisplayName("Fund Expense Amount")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##;-0.##;\\}")]
		public decimal? FundExpenseAmount { get; set; }

		[DisplayName("FromDate")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime? FromDate { get; set; }

		[DisplayName("To")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime? ToDate { get; set; }

		[DisplayName("Management Fees Interest")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##;-0.##;\\}")]
		public decimal? ManagementFeeInterest { get; set; }

		[DisplayName("Invested Amount Interest")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##;-0.##;\\}")]
		public decimal? InvestedAmountInterest { get; set; }

		[DisplayName("Fund Expense Amount")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##;-0.##;\\}")]
		public decimal? FundExpenses { get; set; }

		[DisplayName("Invested Amount")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##;-0.##;\\}")]
		public decimal? InvestedAmount { get; set; }

		public int? InvestorCount { get; set; }

		public bool? AddManagementFees { get; set; }

		public bool? AddFundExpenses { get; set; }

		public IEnumerable<CapitalCallLineItemModel> CapitalCallLineItems { get; set; }

		public int CapitalCallLineItemsCount { get; set; }
	}

	public class CapitalCallLineItemModel {

		[Required(ErrorMessage = "CapitalCallLineItemID is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CapitalCallLineItemID is required")]
		public int CapitalCallID { get; set; }

		[Required(ErrorMessage = "CapitalCallLineItemID is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CapitalCallLineItemID is required")]
		public int CapitalCallLineItemID { get; set; }

		public string InvestorName { get; set; }

		public decimal CapitalAmountCalled { get; set; }

		public decimal InvestmentAmount { get; set; }

		public decimal? NewInvestmentAmount { get; set; }

		public decimal? ExistingInvestmentAmount { get; set; }

		public decimal? ManagementFees { get; set; }

		public decimal? FundExpenses { get; set; }

		public decimal? ManagementFeeInterest { get; set; }

		public decimal? InvestedAmountInterest { get; set; }
	}
}