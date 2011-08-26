using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DeepBlue.Models.Report {
	public class CapitalCallSummaryModel {

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		[DisplayName("Fund")]
		public int FundId { get; set; }

		[Required(ErrorMessage = "Capital Call Due Date is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Capital Call Due Date is required")]
		[DisplayName("Capital Call Due Date")]
		public int CapitalCallId { get; set; }

		public List<SelectListItem> CapitalCalls { get; set; }

	}
}