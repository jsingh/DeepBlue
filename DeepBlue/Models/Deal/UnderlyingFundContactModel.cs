using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class UnderlyingFundContactModel {

		public int UnderlyingFundContactId { get; set; }
		public int UnderlyingFundId { get; set; }
		public int ContactId { get; set; }

		[StringLength(100, ErrorMessage = "Contact Name must be under 100 characters.")]
		[DisplayName("Contact Name")]
		public string ContactName { get; set; }

		[StringLength(200, ErrorMessage = "Web Address must be under 200 characters.")]
		[DisplayName("Web Address")]
		public string WebAddress { get; set; }

		[StringLength(200, ErrorMessage = "Mailing Address must be under 200 characters.")]
		[DisplayName("Mailing Address")]
		public string Address { get; set; }

		[StringLength(200, ErrorMessage = "Phone must be under 200 characters.")]
		[DisplayName("Phone")]
		public string Phone { get; set; }

		[StringLength(200, ErrorMessage = "Email must be under 200 characters.")]
		[DisplayName("Email")]
		[EmailAttribute(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }

		[StringLength(100, ErrorMessage = "Title must be under 100 characters.")]
		[DisplayName("Title")]
		public string ContactTitle { get; set; }

		[StringLength(500, ErrorMessage = "Notes must be under 100 characters.")]
		[DisplayName("Notes")]
		public string ContactNotes { get; set; }

		[StringLength(200, ErrorMessage = "Web User name must be under 200 characters.")]
		[DisplayName("Web User name")]
		public string WebUsername { get; set; }

		[StringLength(200, ErrorMessage = "Web Password must be under 200 characters.")]
		[DisplayName("Web Password")]
		public string WebPassword { get; set; }

		public bool ChangeWebPassword { get; set; }
	}
}