using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditShareClassTypeModel{

		public int ShareClassTypeId { get; set; }
		
		[Required(ErrorMessage = "ShareClass is required.")]
		[StringLength(100, ErrorMessage = "ShareClass must be under 100 characters.")]
		[RemoteUID_(Action = "ShareClassAvailable", Controller = "Admin", ValidateParameterName = "ShareClass", Params = new string[] { "ShareClassTypeId" })]
		[DisplayName("ShareClass:")]
		public string ShareClass { get; set; }

		[DisplayName("Enable:")]
		public bool Enabled { get; set; }
 
	}
}