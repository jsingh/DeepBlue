using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace DeepBlue.Models.Deal {
	public class EquitySplitModel : SecurityActivityModel {
	 
		[Required(ErrorMessage = "Direct is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Direct is required")]
		[DisplayName("Direct-")]
		public int EquityId { get; set; }

		[Required(ErrorMessage = "Security Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Security Type is required")]
		[DisplayName("Direct Type-")]
		public int SecurityTypeId { get; set; }

		[DisplayName("Direct Symbol-")]
		public string Symbol { get; set; }

	}
}