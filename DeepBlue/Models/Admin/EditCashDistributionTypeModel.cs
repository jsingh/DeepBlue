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
			Name = string.Empty;
			Enabled = false;
		}

		public int CashDistributionTypeId { get; set; }

		[Required(ErrorMessage = "Cash Distribution Type is required")]
		[StringLength(50, ErrorMessage = "Cash Distribution Type must be under 50 characters.")]
		[DisplayName("Name:")]
		public string Name { get; set; }

		[DisplayName("Enable:")]
		public bool Enabled { get; set; }

	}
}