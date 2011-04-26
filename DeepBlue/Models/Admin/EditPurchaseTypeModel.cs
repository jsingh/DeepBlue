using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.ComponentModel;

namespace DeepBlue.Models.Admin {
	public class EditPurchaseTypeModel {
		public EditPurchaseTypeModel() {
			PurchaseTypeId = 0;
			Name = string.Empty;
		}

		public int PurchaseTypeId { get; set; }

		[Required(ErrorMessage = "Name is required")]
		[StringLength(50, ErrorMessage = "Name must be under 50 characters.")]
		[RemoteUID_(Action = "PurchaseTypeNameAvailable", Controller = "Admin", ValidateParameterName = "Name", Params = new string[] { "PurchaseTypeId" })]
		[DisplayName("Name:")]
		public string Name { get; set; }
	}
}