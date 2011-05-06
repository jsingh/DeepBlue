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

		[Required(ErrorMessage = "Record Date is required")]
		[DateRange()]
		[DisplayName("RecordDate:")]
		public DateTime? RecordDate { get; set; }

		[Range((double)0, (double)decimal.MaxValue, ErrorMessage = "FMV is required")]
		[DisplayName("FMV:")]
		public decimal? FMV { get; set; }

		[Range((double)0, (double)100, ErrorMessage = "Percent must be under 100%.")]
		[DisplayName("Percent:")]
		public decimal? Percent { get; set; }

		[Range((int)0, int.MaxValue, ErrorMessage = "NumberOfShares is required")]
		[DisplayName("NoOfShares:")]
		public int? NumberOfShares { get; set; }

		[Required(ErrorMessage = "Purchase Price is required")]
		[Range((double)0, (double)decimal.MaxValue, ErrorMessage = "Purchase Price is required")]
		[DisplayName("Purchase Price:")]
		public decimal PurchasePrice { get; set; }

		[DisplayName("Tax Cost Basis:")]
		public decimal? TaxCostBase { get; set; }

		[DisplayName("Tax Cost Date:")]
		public DateTime? TaxCostDate { get; set; }

		public List<SelectListItem> Equities { get; set; }

		public List<SelectListItem> FixedIncomes { get; set; }

		public string FundName { get; set; }

		public DateTime DealCloseDate { get; set; }

		public List<SelectListItem> Issuers { get; set; }

		public List<SelectListItem> SecurityTypes { get; set; }
	}
}