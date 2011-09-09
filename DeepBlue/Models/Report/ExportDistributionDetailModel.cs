using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Report {
	public class ExportDistributionDetailModel {

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		[Required(ErrorMessage = "Export Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Export Type is required")]
		public int ExportTypeId { get; set; }

		public List<DistributionReportDetail> DistributionReportDetails { get; set; }
	}
}