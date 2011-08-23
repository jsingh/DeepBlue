using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Account {
	public class LogOnModel {

		[Required(ErrorMessage="Username is required")]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required(ErrorMessage="Password is required")]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }

		public string Errors { get; set; }
	}
}