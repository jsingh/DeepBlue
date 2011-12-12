using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditSellerTypeModel{

		public int SellerTypeId { get; set; }
		
		[Required(ErrorMessage = "Seller Type is required.")]
		[StringLength(100, ErrorMessage = "Seller Type must be under 100 characters.")]
		[DisplayName("Seller Type")]
		public string SellerType { get; set; }

		[DisplayName("Enable")]
		public bool Enabled { get; set; }
 
	}
}