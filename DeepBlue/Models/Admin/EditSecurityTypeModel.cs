using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.ComponentModel;

namespace DeepBlue.Models.Admin {
	public class EditSecurityTypeModel {
		public EditSecurityTypeModel() {
			SecurityTypeId = 0;
			Name = string.Empty;
		}

		public int SecurityTypeId { get; set; }

		[Required(ErrorMessage = "Name is required")]
		[StringLength(100, ErrorMessage = "Name must be under 100 characters.")]
		[DisplayName("Name")]
		public string Name { get; set; }

		[DisplayName("Enabled")]
		public bool Enabled { get; set; }
	}
}