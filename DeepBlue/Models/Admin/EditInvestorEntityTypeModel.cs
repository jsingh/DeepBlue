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

		[Required(ErrorMessage = "Investor EntityType Name is required")]
		[RemoteUID_(Action = "InvestorEntityTypeNameAvailable", Controller = "Admin", ValidateParameterName = "InvestorEntityTypeName", Params = new string[] { "InvestorEntityTypeId" })]
		[DisplayName("Investor EntityType Name:")]
		public string InvestorEntityTypeName { get; set; }

		[DisplayName("Enable:")]
		public bool Enabled { get; set; }

	}
}