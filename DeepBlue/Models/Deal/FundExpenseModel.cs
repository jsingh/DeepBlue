using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class FundExpenseModel {

		public FundExpenseModel() {
			FundExpenseId = 0;
			FundExpenseTypeId = 0;
			FundId = 0;
			Amount = 0;
			Date = DateTime.Now;
		}
		
		public int FundExpenseId { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		[Required(ErrorMessage = "Expense Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Expense Type is required")]
		[DisplayName("Expense Type")]
		public int FundExpenseTypeId { get; set; }

		public string FundExpenseType { get; set; }

		[Required(ErrorMessage = "Amount is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Amount is required")]
		[DisplayName("Amount")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.##;-0.##;\\}")]
		public decimal Amount { get; set; }

		[Required(ErrorMessage = "Date is required")]
		[DisplayName("Date")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DateRange(ErrorMessage="Date is required")]
		public DateTime? Date { get; set; }

		public List<SelectListItem> FundExpenseTypes { get; set; }

	}
}