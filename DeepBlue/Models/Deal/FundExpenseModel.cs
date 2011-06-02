using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace DeepBlue.Models.Deal {
	public class FundExpenseModel {

		public int FundExpenseId { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		[Required(ErrorMessage = "Fund Expense Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund Expense Type is required")]
		[DisplayName("Fund Expense Type-")]
		public int FundExpenseTypeId { get; set; }

		[Required(ErrorMessage = "Expense Amount is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Expense Amount is required")]
		[DisplayName("Expense Amount-")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##;-0.##;\\}")]
		public decimal Amount { get; set; }

		public DateTime? Date { get; set; }

		public List<SelectListItem> FundExpenseTypes { get; set; }

		public List<ExpenseToDealModel> ExpenseToDeals { get; set; }
	}
}