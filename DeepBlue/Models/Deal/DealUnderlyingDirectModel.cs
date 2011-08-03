using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.Web.Mvc;
using System.ComponentModel;

namespace DeepBlue.Models.Deal {
	public class DealUnderlyingDirectModel {
		
		public int? DealUnderlyingDirectId { get; set; }

		public string IssuerName { get; set; }

		[Required(ErrorMessage = "Company is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Company is required")]
		[DisplayName("Company:")]
		public int IssuerId { get; set; }

		public string SecurityType { get; set; }

		[Required(ErrorMessage = "Security Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Security Type is required")]
		[DisplayName("SecurityType:")]
		public int SecurityTypeId { get; set; }

		public string Security { get; set; }

		[Required(ErrorMessage = "Security is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Security is required")]
		[DisplayName("Security:")]
		public int SecurityId { get; set; }

		[Required(ErrorMessage = "Deal is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
		public int DealId { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		[Required(ErrorMessage = "Record Date is required")]
		[DateRange()]
		[DisplayName("RecordDate:")]
		public DateTime? RecordDate { get; set; }

		[Required(ErrorMessage = "FMV is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "FMV is required")]
		[DisplayName("FMV:")]
		public decimal? FMV { get; set; }

		[DisplayName("Percent:")]
		public decimal? Percent { get; set; }

		[Required(ErrorMessage = "Number Of Shares is required")]
		[Range((int)1, int.MaxValue, ErrorMessage = "Number Of Shares is required")]
		[DisplayName("NoOfShares:")]
		public int? NumberOfShares { get; set; }

		[Required(ErrorMessage = "Purchase Price is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Purchase Price is required")]
		[DisplayName("Purchase Price:")]
		public decimal PurchasePrice { get; set; }

		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Purchase Price is required")]
		[DisplayName("Tax Cost Basis:")]
		public decimal? TaxCostBase { get; set; }

		[DisplayName("Tax Cost Date:")]
		[DateRange()]
		public DateTime? TaxCostDate { get; set; }

		public int? DealClosingId { get; set; }

		public decimal? AdjustedFMV { get; set; }

		public bool IsClose { get; set; }
		
		public List<SelectListItem> Equities { get; set; }

		public List<SelectListItem> FixedIncomes { get; set; }

		public string FundName { get; set; }

		public DateTime DealCloseDate { get; set; }

		public List<SelectListItem> Issuers { get; set; }

		public List<SelectListItem> SecurityTypes { get; set; }
	}
}