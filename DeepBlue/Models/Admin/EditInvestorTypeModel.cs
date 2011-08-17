using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;


namespace DeepBlue.Models.Admin {
	public class EditInvestorTypeModel {
		public EditInvestorTypeModel() {
			InvestorTypeId = 0;
			InvestorTypeName = string.Empty;
			Enabled = false;
		}

		public int InvestorTypeId { get; set; }

		[Required(ErrorMessage = "Investor Type is required")]
		[StringLength(20, ErrorMessage = "Investor Type must be under 20 characters.")]
		[DisplayName("Investor Type Name:")]
		public string InvestorTypeName { get; set; }

		[DisplayName("Enable:")]
		public bool Enabled { get; set; }

	}
}