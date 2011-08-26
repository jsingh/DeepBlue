using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class SecurityConversionModel : SecurityActivityModel {

		public SecurityConversionModel() {
			ConversionDate = DateTime.Now;
		}

		[Required(ErrorMessage = "Old Direct is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Old Direct is required")]
		[DisplayName("Old Direct")]
		public int OldSecurityId { get; set; }

		[Required(ErrorMessage = "Old Direct Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Old Direct Type is required")]
		[DisplayName("Old Direct Type")]
		public int OldSecurityTypeId { get; set; }

		[DisplayName("Old Direct Symbol")]
		public string OldSymbol { get; set; }

		[Required(ErrorMessage = "New Direct is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "New Direct is required")]
		[DisplayName("New Direct")]
		public int NewSecurityId { get; set; }

		[Required(ErrorMessage = "New Direct Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "New Direct Type is required")]
		[DisplayName("New Direct Type")]
		public int NewSecurityTypeId { get; set; }

		[DisplayName("New Direct Symbol")]
		public string NewSymbol { get; set; }

		[Required(ErrorMessage = "Conversion Date is required")]
		[DateRange(ErrorMessage = "Conversion Date is required")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DisplayName("Conversion Date")]
		public DateTime ConversionDate { get; set; }
	}
}