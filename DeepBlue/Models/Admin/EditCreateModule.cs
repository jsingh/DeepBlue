using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditModule {
		public EditModule() {
		}

		public int ModuleID { get; set; }

		[Required(ErrorMessage = "Module Name is required.")]
		[StringLength(50, ErrorMessage = "Module Name must be under 50 characters.")]
		[DisplayName("Name")]
		public string ModuleName { get; set; }
	}
}