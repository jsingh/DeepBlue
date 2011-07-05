using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {

	public class UnderlyingFundStockDistributionModel {

		public UnderlyingFundStockDistributionModel() {
			UnderlyingFundStockDistributionId = 0;
		}

		public int UnderlyingFundStockDistributionId { get; set; }

		public int UnderlyingFundId { get; set; }

		public string UnderlyingFundName { get; set; }

		public int FundId { get; set; }

		public string FundName { get; set; }
				
		public int SecurityId { get; set; }

		public int SecurityTypeId { get; set; }
		 
		public bool IsManualStockDistribution { get; set; }

	}


	public class StockDealUnderlyingDirectModel {

		public int DealUnderlyingDirectId { get; set; }

		public int SecurityId { get; set; }

		public int SecurityTypeId { get; set; }
		
		public int? NumberOfShares { get; set; }

		public decimal? PurchasePrice { get; set; }

		public decimal? FMV { get; set; }

		public decimal? TaxCostBase { get; set; }

		public DateTime? TaxCostDate { get; set; }

		public DateTime? NoticeDate { get; set; }

		public DateTime? DistributionDate { get; set; }

		public int DealId { get; set; }

		public int DealNumber { get; set; }

		public string DealName { get; set; }

		public string DirectName { get; set; }
	}
	 
}