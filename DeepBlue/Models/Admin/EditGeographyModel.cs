using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditGeographyModel{

		public int GeographyId { get; set; }

		[Required(ErrorMessage = "Geography is required.")]
		[StringLength(100, ErrorMessage = "Geography must be under 100 characters.")]
		[DisplayName("Geography:")]
		public string Geography { get; set; }

		[DisplayName("Enable:")]
		public bool Enabled { get; set; }
 
	}
}