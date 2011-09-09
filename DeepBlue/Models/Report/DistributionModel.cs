using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Report {

	public class DistributionModel {

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		[DisplayName("Fund")]
		public int FundId { get; set; }

		[DisplayName("Start Date")]
		[DateRange(ErrorMessage = "Invalid Start Date")]
		public DateTime? StartDate { get; set; }

		[DisplayName("End Date")]
		[DateRange(ErrorMessage = "Invalid End Date")]
		public DateTime? EndDate { get; set; }

	}
}