using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Report {
	public class DealOriginationReportModel {

		public bool IsTemplateDisplay { get; set; }

		public string FundName { get; set; }

		public int FundId { get; set; }

		public int DealNumber { get; set; }

		public string DealName { get; set; }

		public string Contact { get; set; }

		public decimal? GrossPurchasePrice { get; set; }

		public decimal? NetPurchasePrice { get; set; }

		public IEnumerable<DealOrganizationFundDetailModel> Details { get; set; }
 
	}
}