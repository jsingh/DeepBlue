using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditIndustryModel{

		public int IndustryId { get; set; }

		[Required(ErrorMessage = "Industry is required.")]
		[StringLength(100, ErrorMessage = "Industry must be under 100 characters.")]
		[DisplayName("Industry:")]
		public string Industry { get; set; }

		[DisplayName("Enable:")]
		public bool Enabled { get; set; }
 
	}
}