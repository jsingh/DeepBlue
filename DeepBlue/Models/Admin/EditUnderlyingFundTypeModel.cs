using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditUnderlyingFundTypeModel{
		public int UnderlyingFundTypeId { get; set; }

		[Required(ErrorMessage = "Underlying Fund Type is required.")]
		[StringLength(50, ErrorMessage = "Underlying Fund Type must be under 50 characters.")]
		[DisplayName("Name:")]
		public string Name { get; set; }
	}
}