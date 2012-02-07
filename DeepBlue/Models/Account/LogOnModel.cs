using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DeepBlue.Models.Account {
	public class LogOnModel {

		[Required(ErrorMessage="Username is required")]
		[DisplayName("User Name")]
		public string UserName { get; set; }

		[Required(ErrorMessage="Password is required")]
		[DataType(DataType.Password)]
		[DisplayName("Password")]
		public string Password { get; set; }

		[Required(ErrorMessage = "EntityCode is required")]
		[DataType(DataType.Password)]
		[DisplayName("Entity Code")]
		public string EntityCode { get; set; }

		[DisplayName("Remember Me")]
		public bool RememberMe { get; set; }

		public string ReturnUrl { get; set; }

		public string Errors { get; set; }
 
	}
}