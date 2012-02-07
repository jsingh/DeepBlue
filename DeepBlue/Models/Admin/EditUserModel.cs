using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditUserModel{

		public int UserId { get; set; }

		[Required(ErrorMessage = "Username is required")]
		[StringLength(50, ErrorMessage = "Username must be under 50 characters.")]
		public string Login { get; set; }

		[Required(ErrorMessage = "FirstName is required")]
		[StringLength(30, ErrorMessage = "FirstName must be under 30 characters.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "LastName is required")]
		[StringLength(30, ErrorMessage = "LastName must be under 30 characters.")]
		public string LastName { get; set; }

		[StringLength(30, ErrorMessage = "MiddleName must be under 30 characters.")]
		public string MiddleName { get; set; }

		public string Password { get; set; }

		public bool ChangePassword { get; set; }

		[Required(ErrorMessage = "Email is required")]
		[StringLength(50, ErrorMessage = "Email must be under 50 characters.")]
		[Email(ErrorMessage="Invalid Email")]
		public string Email { get; set; }

		[StringLength(25, ErrorMessage = "PhoneNumber must be under 25 characters.")]
		public string PhoneNumber { get; set; }

		public bool Enabled { get; set; }

		public bool IsAdmin { get; set; }

		public List<SelectListItem> Entities { get; set; } 

	}
}