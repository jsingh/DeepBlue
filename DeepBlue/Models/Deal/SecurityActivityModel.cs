using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace DeepBlue.Models.Deal {
	public class SecurityActivityModel {

		[Required(ErrorMessage = "Corporate Action is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Corporate Action is required")]
		[DisplayName("Corporate Action-")]
		public int ActivityTypeId { get; set; }

		[Required(ErrorMessage = "Stock is required")]
		[Range((int)1, int.MaxValue, ErrorMessage = "Stock is required")]
		[DisplayName("1 Stock-")]
		public int? SplitFactor { get; set; }

		public List<SelectListItem> SecurityTypes { get; set; }
	}
}