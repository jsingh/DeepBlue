using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class DealSellerDetailModel {

		public DealSellerDetailModel() {
			DealId = 0;
			SellerContactId = 0;
			ContactName = string.Empty;
			Phone = string.Empty;
			Fax = string.Empty;
			SellerName = string.Empty;
			CompanyName = string.Empty;
			Email = string.Empty;
		}

		
		/* Seller Information */
		public int DealId { get; set; }

		public int SellerContactId { get; set; }

		[StringLength(100, ErrorMessage = "Contact Name must be under 100 characters.")]
		[DisplayName("Cotact Name-")]
		public string ContactName { get; set; }

		[DisplayName("Phone-")]
		[StringLength(200, ErrorMessage = "Phone must be under 200 characters.")]
		public string Phone { get; set; }

		[DisplayName("Fax-")]
		[StringLength(200, ErrorMessage = "Fax must be under 200 characters.")]
		public string Fax { get; set; }

		[DisplayName("Seller Name-")]
		[StringLength(30, ErrorMessage = "Seller must be under 30 characters.")]
		public string SellerName { get; set; }

		[DisplayName("Cotact Company-")]
		public string CompanyName { get; set; }
		
		[EmailAttribute(ErrorMessage="Invalid Email")]
		[DisplayName("Email-")]
		public string Email { get; set; }
	}
}