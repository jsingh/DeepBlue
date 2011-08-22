using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DeepBlue.Models.Report {
	public class CashDistributionSummaryModel {

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		[DisplayName("Fund:")]
		public int FundId { get; set; }

		[Required(ErrorMessage = "Cash Distribution Date is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Cash Distribution Date is required")]
		[DisplayName("Cash Distribution Date:")]
		public int CapitalDistributionId { get; set; }

		public List<SelectListItem> CapitalDistributions { get; set; }

		
	}
}