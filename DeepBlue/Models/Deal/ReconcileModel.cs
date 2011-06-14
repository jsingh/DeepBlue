using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Deal {
	public class ReconcileModel {

		public ReconcileModel() {
			UnderlyingFundId = 0;
			FundId = 0;
			PageIndex = 1;
			PageSize = 5;
		}

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime? FromDate { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime? ToDate { get; set; }

		public int? UnderlyingFundId { get; set; }

		public int? FundId { get; set; }

		public int PageIndex { get; set; }

		public int PageSize { get; set; }
	}



	public class ReconcileListModel {

		public int EventType { get; set; }

		public string UnderlyingFundName { get; set; }

		public string FundName { get; set; }

		public decimal CapitalCallAmount { get; set; }

		public decimal DistributionAmount { get; set; }

		public DateTime? PaymentOrReceivedDate { get; set; }

		public bool PaidOnOrReceivedOn { get; set; }
	}

	public class ReconcilesTotalModel {


	}
}