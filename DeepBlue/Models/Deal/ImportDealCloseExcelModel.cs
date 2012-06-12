using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
 

	public class ImportDealCloseUnderlyingFundExcelModel : ImportExcelPagingModel {

		public string DealCloseUnderlyingFundTableName { get; set; }

		public string FundName { get; set; }

		public string DealName { get; set; }

		public string CloseDate { get; set; }

		public string UnderlyingFundName { get; set; }
		
		public string GrossPurchasePrice { get; set; }

		public string NetPurchasePrice { get; set; }

		public string CapitalCommitment { get; set; }
		
		public string UnfundedAmount { get; set; }

		public string EffectiveDate { get; set; }

		public string RecordDate { get; set; }

		public string SessionKey { get; set; }

	}

	public class ImportDealCloseUnderlyingDirectExcelModel : ImportExcelPagingModel {

		public string DealCloseUnderlyingDirectTableName { get; set; }

		public string FundName { get; set; }

		public string DealName { get; set; }

		public string CloseDate { get; set; }

		public string CompanyName { get; set;}

		public string Symbol { get; set; }

		public string SecurityType { get; set; }
		
		public string NoOfShares { get; set; }
		
		public string PurchasePrice { get; set; }
		
		public string FairMarketValue { get; set; }
		
		public string TaxCostBasisPerShare { get; set; }
		
		public string TaxCostDate { get; set; }

		public string RecordDate { get; set; }

		public string SessionKey { get; set; }

	}

}