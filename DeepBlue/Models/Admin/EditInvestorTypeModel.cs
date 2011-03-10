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

		[Required(ErrorMessage = "Investor Type Name is required")]
		[RemoteUID_(Action = "InvestorTypeNameAvailable", Controller = "Admin", ValidateParameterName = "InvestorTypeName", Params = new string[] { "InvestorTypeId" })]
		[DisplayName("Investor Type Name:")]
		public string InvestorTypeName { get; set; }

		[DisplayName("Enable:")]
		public bool Enabled { get; set; }

	}
}