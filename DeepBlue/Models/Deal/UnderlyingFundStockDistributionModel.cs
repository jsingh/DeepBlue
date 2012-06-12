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

		public string UnderlyingFundName { get; set; }

		public string FundName { get; set; }

		[Required(ErrorMessage = "UnderlyingFund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "UnderlyingFund is required")]
		public int UnderlyingFundId { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		[Required(ErrorMessage = "Equity is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Equity is required")]
		public int SecurityId { get; set; }

		[Required(ErrorMessage = "Security Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Security Type is required")]
		public int SecurityTypeId { get; set; }

		[Required(ErrorMessage = "NumberOfShares is required")]
		[Range((int)1, int.MaxValue, ErrorMessage = "NumberOfShares is required")]
		public int NumberOfShares { get; set; }

		[Required(ErrorMessage = "Purchase Price is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Purchase Price is required")]
		public decimal PurchasePrice { get; set; }

		[Required(ErrorMessage = "Notice Date is required")]
		[DateRange(ErrorMessage = "Notice Date is required")]
		public DateTime NoticeDate { get; set; }

		[Required(ErrorMessage = "Distribution Date is required")]
		[DateRange(ErrorMessage = "Distribution Date is required")]
		public DateTime DistributionDate { get; set; }

		[Required(ErrorMessage = "Tax Cost Base is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Tax Cost Base is required")]
		public decimal TaxCostBase { get; set; }

		[Required(ErrorMessage = "Tax Cost Date is required")]
		[DateRange(ErrorMessage = "Tax Cost Date is required")]
		public DateTime TaxCostDate { get; set; }

		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Broker is required")]
		public int? BrokerID { get; set; }

		public string Notes { get; set; }

		public bool IsManualStockDistribution { get; set; }

		public IEnumerable<StockDistributionLineItemModel> Deals { get; set; }

	}


	public class StockDistributionLineItemModel {

		public int DealId { get; set; }

		public int FundId { get; set; }

		public string DealName { get; set; }

		public int DealNumber { get; set; }

		public decimal? CommitmentAmount { get; set; }

	}

}