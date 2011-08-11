using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DeepBlue.Models.Report {
	public class ExportDealOriginationModel {

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		[Required(ErrorMessage = "Deal is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
		public int DealId { get; set; }

		[Required(ErrorMessage = "Export Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Export Type is required")]
		public int ExportTypeId { get; set; }

		public DealOriginationReportModel DealOriginationReportModel { get; set; }

	}
}