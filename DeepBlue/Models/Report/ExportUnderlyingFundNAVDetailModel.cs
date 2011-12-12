using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Report {
	public class ExportUnderlyingFundNAVDetailModel {

		[Required(ErrorMessage = "Underlying Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Underlying Fund is required")]
		public int UnderlyingFundId { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		[Required(ErrorMessage = "Export Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Export Type is required")]
		public int ExportTypeId { get; set; }

		public List<UnderlyingFundNAVReportDetail> UnderlyingFundNAVReportDetails { get; set; }
	}
}