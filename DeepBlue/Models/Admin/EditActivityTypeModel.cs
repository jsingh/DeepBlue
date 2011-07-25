using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditFundExpenseTypeModel {
		public EditFundExpenseTypeModel() {
			FundExpenseTypeId = 0;
			Name = string.Empty;
		}

		public int FundExpenseTypeId { get; set; }

		[Required(ErrorMessage = "Fund Expense Type is required")]
		[StringLength(50, ErrorMessage = "Fund Expense Type must be under 50 characters.")]
		[DisplayName("Name:")]
		public string Name { get; set; }
	}
}