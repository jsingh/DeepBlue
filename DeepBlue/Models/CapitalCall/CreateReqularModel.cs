using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {
	public class CreateReqularModel {

		[DisplayName("Fund:")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		[DisplayName("Capital Amount:")]
		[Required(ErrorMessage = "Capital Call Amount is required")]
		[Range(typeof(decimal),"1", "79228162514264337593543950335", ErrorMessage = "Capital Call Amount is required")]
		public decimal CapitalAmountCalled { get; set; }

		public string CapitalCallNumber { get; set; }

		[DisplayName("Capital Call Date:")]
		[Required(ErrorMessage = "Capital Call Date is required")]
		[DateRange()]
		public DateTime CapitalCallDate { get; set; }

		[DisplayName("Capital Call Due Date:")]
		[Required(ErrorMessage = "Capital Call Due Date is required")]
		[DateRange()]
		public DateTime CapitalCallDueDate { get; set; }

		[DisplayName("New Investment Amount:")]
		[Required(ErrorMessage = "New Investment Amount is required")]
		[Range(typeof(decimal),"1", "79228162514264337593543950335", ErrorMessage = "New Investment Amount is required")]
		public decimal NewInvestmentAmount { get; set; }

		[DisplayName("Existing Investment Amount:")]
		public decimal? ExistingInvestmentAmount { get; set; }

		public decimal? ManagementFees { get; set; }

		public decimal? FundExpenseAmount { get; set; }

		[DisplayName("FromDate:")]
		public DateTime? FromDate { get; set; }

		[DisplayName("To:")]
		public DateTime? ToDate { get; set; }

		public bool AddManagementFees { get; set; }

		public bool AddFundExpenses { get; set; }

	}
}