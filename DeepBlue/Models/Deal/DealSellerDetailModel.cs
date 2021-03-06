﻿using System;
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
		[Required(ErrorMessage = "Deal is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
		public int DealId { get; set; }

		public int SellerContactId { get; set; }

		[StringLength(100, ErrorMessage = "Contact Name must be under 100 characters.")]
		[DisplayName("Contact Name")]
		public string ContactName { get; set; }
		
		[StringLength(200, ErrorMessage = "Phone must be under 200 characters.")]
		[DisplayName("Phone")]
		public string Phone { get; set; }
		
		[StringLength(200, ErrorMessage = "Fax must be under 200 characters.")]
		[DisplayName("Fax")]
		public string Fax { get; set; }

		[StringLength(30, ErrorMessage = "Seller must be under 30 characters.")]
		[DisplayName("Seller Name")]
		public string SellerName { get; set; }

		public string SellerType { get; set; }

		public int? SellerTypeId { get; set; }
		
		[StringLength(200, ErrorMessage = "Company Name must be under 200 characters.")]
		[DisplayName("Contact Company")]
		public string CompanyName { get; set; }
		
		[EmailAttribute(ErrorMessage="Invalid Email")]
		[StringLength(200, ErrorMessage = "Email must be under 200 characters.")]
		[DisplayName("Email")]
		public string Email { get; set; }
	}
}