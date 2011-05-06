using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Deal.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DeepBlue.Models.Deal {
	public class DealExportModel : DealReportModel {

		public DealExportModel() {
			FundId = 0;
			SortName = string.Empty;
			SortOrder = "asc";
			Deals = new List<DealReportModel>();
			IsPrint = false;
		}

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		[DisplayName("Fund Name -")]
		public int FundId { get; set; }

		public string SortName { get; set; }

		public string SortOrder { get; set; }

		public List<DealReportModel> Deals { get; set; }

		public bool IsPrint { get; set; }
	}
}