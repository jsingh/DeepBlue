using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Deal {
	public class DealCloseListModel {

		public int DealClosingId { get; set; }

		public int? DealNumber { get; set; }

		public string DealCloseName { get { return "Deal Close" + this.DealNumber.ToString(); } }

		public string DealName { get; set; }

		public string FundName { get; set; }

		public DateTime CloseDate { get; set; }

		public decimal? TotalGrossPurchasePrice { get; set; }

		public decimal? TotalPostRecordCapitalCall { get; set; }

		public decimal? TotalPostRecordDateDistribution { get; set; }

		public decimal? TotalNetPurchasePrice {
			get {
				return ((this.TotalGrossPurchasePrice ?? 0) + (this.TotalPostRecordCapitalCall ?? 0) - (this.TotalPostRecordDateDistribution ?? 0));
			}
		}

	}
		
}