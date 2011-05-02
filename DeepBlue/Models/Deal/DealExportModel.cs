using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Deal.Enums;

namespace DeepBlue.Models.Deal {
	public class DealExportModel : DealReportModel {

		public ExportType ExportType { get; set; }

		public List<DealReportModel> Deals { get; set; }
	}
}