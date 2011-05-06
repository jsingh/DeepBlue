using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Deal {

	public class DealReportModel {

		public DealReportModel() {
			DealId = 0;
			DealNumber = 0;
			DealName = string.Empty;
			FundName = string.Empty;
			SellerName = string.Empty;
			DealUnderlyingFunds = new List<DealUnderlyingFundDetail>();
			DealUnderlyingDirects = new List<DealUnderlyingDirectDetail>();
		}

		public int DealId { get; set; }

		public int DealNumber { get; set; }

		public string DealName { get; set; }

		public string FundName { get; set; }

		public string SellerName { get; set; }

		public List<DealUnderlyingFundDetail> DealUnderlyingFunds { get; set; }

		public List<DealUnderlyingDirectDetail> DealUnderlyingDirects { get; set; }
	}

	public class DealUnderlyingDetail {

		public DealUnderlyingDetail() {
			DealUnderlyingFunds = new List<DealUnderlyingFundDetail>();
			DealUnderlyingDirects = new List<DealUnderlyingDirectDetail>();
		}

		public List<DealUnderlyingFundDetail> DealUnderlyingFunds { get; set; }

		public List<DealUnderlyingDirectDetail> DealUnderlyingDirects { get; set; }
	}

	public class DealUnderlyingFundDetail {
		
		public string FundName { get; set; }

		public decimal? NAV { get; set; }

		public decimal? Commitment { get; set; }

		public DateTime? RecordDate { get; set; }
	}

	public class DealUnderlyingDirectDetail {

		public string Company { get; set; }

		public string Security { get; set; }

		public int? NoOfShares { get; set; }

		public decimal? Percentage { get; set; }

		public decimal? FMV { get; set; }

		public DateTime? RecordDate { get; set; }
	}
}