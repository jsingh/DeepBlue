using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.ComponentModel;

namespace DeepBlue.Models.Admin {
	public class EditDealClosingCostTypeModel {
		public EditDealClosingCostTypeModel() {
			DealClosingCostTypeId = 0;
			Name = string.Empty;
		}

		public int DealClosingCostTypeId { get; set; }

		[Required(ErrorMessage = "Name is required")]
		[StringLength(50, ErrorMessage = "Name must be under 50 characters.")]
		[RemoteUID_(Action = "DealClosingCostTypeNameAvailable", Controller = "Admin", ValidateParameterName = "Name", Params = new string[] { "DealClosingCostTypeId" })]
		[DisplayName("Name:")]
		public string Name { get; set; }
	}
}