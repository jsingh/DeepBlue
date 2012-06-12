using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {

	public class ImportExcelPagingModel {

		[Required(ErrorMessage = "SessionKey is required")]
		public string SessionKey { get; set; }

		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "PageIndex is required")]
		public int PageIndex { get; set; }

		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "PageSize is required")]
		public int PageSize { get; set; }

	}


	public class ImportExcelError {

		public ImportExcelError() {
			Errors = new List<ErrorInfo>();
		}

		public int RowNumber { get; set; }

		public List<ErrorInfo> Errors { get; set; }

	}


	public class ImportDealExcelModel : ImportExcelPagingModel {

		public string DealTableName { get; set; }

		public string FundName { get; set; }
		
		public string DealName { get; set; }
		
		public string PartnerName { get; set; }
		
		public string PurchaseType { get; set; }
		
		public string ContactName { get; set; }
		
		public string ContactTitle { get; set; }
		
		public string ContactPhoneNumber { get; set; }
		
		public string ContactEmail { get; set; }
		
		public string ContactWebAddress { get; set; }
		
		public string ContactNotes { get; set; }
		
		public string SellerType { get; set; }
		
		public string SellerName { get; set; }
		
		public string SellerContactName { get; set; }
		
		public string SellerPhoneNumber { get; set; }
		
		public string SellerEmail { get; set; }

		public string SellerFax { get; set; }


	}


	public class ImportDealExpenseExcelModel : ImportExcelPagingModel {

		public string DealExpenseTableName { get; set; }

		public string FundName { get; set; }

		public string DealName { get; set; }

		public string Description { get; set; }
		
		public string Amount { get; set; }

		public string Date { get; set; }

		public string SessionKey { get; set; }

	}

	public class ImportDealUnderlyingFundExcelModel : ImportExcelPagingModel {

		public string DealUnderlyingFundTableName { get; set; }

		public string FundName { get; set; }

		public string DealName { get; set; }

		public string UnderlyingFundName { get; set; }
		
		public string GrossPurchasePrice { get; set; }
		
		public string FundNav { get; set; }
		
		public string EffectiveDate { get; set; }
		
		public string CapitalCommitment { get; set; }
		
		public string UnfundedAmount { get; set; }

		public string RecordDate { get; set; }

		public string SessionKey { get; set; }

	}

	public class ImportDealUnderlyingDirectExcelModel : ImportExcelPagingModel {

		public string DealUnderlyingDirectTableName { get; set; }

		public string FundName { get; set; }

		public string DealName { get; set; }

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