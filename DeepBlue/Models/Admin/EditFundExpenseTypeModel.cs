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
	public class EditActivityTypeModel {
		public EditActivityTypeModel() {
			ActivityTypeId = 0;
			Name = string.Empty;
			Enabled = false;
		}

		public int ActivityTypeId { get; set; }

		[Required(ErrorMessage = "Name is required")]
		[StringLength(100, ErrorMessage = "Name must be under 100 characters.")]
		[DisplayName("Name:")]
		public string Name { get; set; }

		[DisplayName("Enable:")]
		public bool Enabled { get; set; }

	}
}