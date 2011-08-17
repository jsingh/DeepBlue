using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
 

namespace DeepBlue.Models.Admin {
	public class EditInvestorEntityTypeModel {
		public EditInvestorEntityTypeModel() {
			InvestorEntityTypeId = 0;
			InvestorEntityTypeName = string.Empty;
			Enabled = false;
		}

		public int InvestorEntityTypeId { get; set; }

		[Required(ErrorMessage = "Investor Entity Type is required.")]
		[StringLength(20, ErrorMessage = "Investor Entity Type must be under 20 characters.")]
		[DisplayName("Investor Entity Type Name:")]
		public string InvestorEntityTypeName { get; set; }

		[DisplayName("Enable:")]
		public bool Enabled { get; set; }

	}
}