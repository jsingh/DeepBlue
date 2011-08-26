using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.ComponentModel;

namespace DeepBlue.Models.Admin {
	public class EditEquityTypeModel {
		public EditEquityTypeModel() {
			EquityTypeId = 0;
			Equity = string.Empty;
		}

		public int EquityTypeId { get; set; }

		[Required(ErrorMessage = "Equity is required")]
		[StringLength(100, ErrorMessage = "Equity must be under 100 characters.")]
		[DisplayName("Equity")]
		public string Equity { get; set; }

		[DisplayName("Enabled")]
		public bool Enabled { get; set; }
	}
}