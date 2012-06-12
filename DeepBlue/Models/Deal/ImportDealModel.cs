using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Deal {
	public class ImportDealModel {

		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "PageIndex is required")]
		public int PageIndex { get; set; }

		[Required(ErrorMessage = "Fund Name is required")]
		public string FundName { get; set; }

		[Required(ErrorMessage = "Deal Name is required")]
		public string DealName { get; set; }

		[Required(ErrorMessage = "Purchase Type is required")]
		public string PurchaseType { get; set; }

		public string PartnerName { get; set; }

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

	public class ImportDealExpenselModel {

		[Required(ErrorMessage = "Fund Name is required")]
		public string FundName { get; set; }

		[Required(ErrorMessage = "Deal Name is required")]
		public string DealName { get; set; }

		[Required(ErrorMessage = "Description is required")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Amount is required")]
		public string Amount { get; set; }

		[Required(ErrorMessage = "Deal Name is required")]
		public string Date { get; set; }

	}

	public class ImportDealUnderlyingFundModel {

		[Required(ErrorMessage = "Fund Name is required")]
		public string FundName { get; set; }

		[Required(ErrorMessage = "Deal Name is required")]
		public string DealName { get; set; }

		public string UnderlyingFundName { get; set; }
		
		public string GrossPurchasePrice { get; set; }
		
		public string FundNav { get; set; }
		
		public string EffectiveDate { get; set; }
		
		public string CapitalCommitment { get; set; }
		
		public string UnfundedAmount { get; set; }

		public string RecordDate { get; set; }

	}

	public class ImportDealUnderlyingDirectModel {

		[Required(ErrorMessage = "Fund Name is required")]
		public string FundName { get; set; }

		[Required(ErrorMessage = "Deal Name is required")]
		public string DealName { get; set; }
		
		public string CompanyName { get; set; }
		
		public string NoOfShares { get; set; }
		
		public string PurchasePrice { get; set; }
		
		public string FaitMarketValue { get; set; }
		
		public string TaxCostBasisPerShare { get; set; }
		 

	}

}