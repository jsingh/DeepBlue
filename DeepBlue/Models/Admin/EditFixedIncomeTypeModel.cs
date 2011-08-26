using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.ComponentModel;

namespace DeepBlue.Models.Admin {
	public class EditFixedIncomeTypeModel {
		public EditFixedIncomeTypeModel() {
			FixedIncomeTypeId = 0;
			FixedIncomeType1 = string.Empty;
		}

		public int FixedIncomeTypeId { get; set; }

		[Required(ErrorMessage = "Fixed Income Type is required")]
		[StringLength(100, ErrorMessage = "Fixed Income Type must be under 100 characters.")]
		[DisplayName("FixedIncomeType")]
		public string FixedIncomeType1 { get; set; }

		[DisplayName("Enabled")]
		public bool Enabled { get; set; }
	}
}