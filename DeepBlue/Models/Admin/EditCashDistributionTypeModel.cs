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
	public class EditCashDistributionTypeModel {
		public EditCashDistributionTypeModel() {
			CashDistributionTypeId = 0;
			CashDistributionTypeName = string.Empty;
			Enabled = false;
		}

		public int CashDistributionTypeId { get; set; }

		[Required(ErrorMessage = "Name is required")]
		[StringLength(50, ErrorMessage = "Name must be under 50 characters.")]
		[RemoteUID_(Action = "CashDistributionTypeNameAvailable", Controller = "Admin", ValidateParameterName = "CashDistributionTypeName", Params = new string[] { "CashDistributionTypeId" })]
		[DisplayName("Name:")]
		public string CashDistributionTypeName { get; set; }

		[DisplayName("Enable:")]
		public bool Enabled { get; set; }

	}
}