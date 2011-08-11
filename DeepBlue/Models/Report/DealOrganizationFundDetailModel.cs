using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Report {
	public class DealOrganizationFundDetailModel {

		public string FundName { get; set; }

		public DateTime? RecordDate { get; set; }

		public DateTime? PurchaseDate { get; set; }

		public decimal? NAV { get; set; }

		public decimal? GrossPurchasePrice { get; set; }

		public decimal? PostRecordAdjustMent { get; set; }

		public decimal? NetPurchasePrice { get; set; }

		public decimal? CommitmentAmount { get; set; }

		public decimal? UnfundedAmount { get; set; }

	}
}