using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DeepBlue.Models.Deal {
	public class SecurityConversionModel : SecurityActivityModel {

		[Required(ErrorMessage = "Old Direct is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Old Direct is required")]
		[DisplayName("Old Direct-")]
		public int OldSecurityId { get; set; }

		[Required(ErrorMessage = "Old Direct Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Old Direct Type is required")]
		[DisplayName("Old Direct Type-")]
		public int OldSecurityTypeId { get; set; }

		[DisplayName("Old Direct Symbol-")]
		public string OldSymbol { get; set; }

		[Required(ErrorMessage = "New Direct is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "New Direct is required")]
		[DisplayName("New Direct-")]
		public int NewSecurityId { get; set; }

		[Required(ErrorMessage = "New Direct Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "New Direct Type is required")]
		[DisplayName("New Direct Type-")]
		public int NewSecurityTypeId { get; set; }

		[DisplayName("New Direct Symbol-")]
		public string NewSymbol { get; set; }
	}
}