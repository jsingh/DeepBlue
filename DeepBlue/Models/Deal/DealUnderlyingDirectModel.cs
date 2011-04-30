using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.Web.Mvc;

namespace DeepBlue.Models.Deal {
	public class DealUnderlyingDirectModel {

		public DealUnderlyingDirectModel() {
			DealUnderlyingDirectId = 0;
			IssuerName = string.Empty;
			IssuerId = 0;
			SecurityType = string.Empty;
			SecurityTypeId = 0;
			Security = string.Empty;
			SecurityId = 0;
			DealId = 0;
			RecordDate = Convert.ToDateTime("01/01/1900");
			FMV = 0;
			Percent = 0;
			NumberOfShares = 0;
		}

		public int? DealUnderlyingDirectId { get; set; }

		public string IssuerName { get; set; }

		[Required(ErrorMessage="Company is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Company is required")]
		public int IssuerId { get; set; }

		public string SecurityType { get; set; }

		[Required(ErrorMessage = "Security Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Security Type is required")]
		public int SecurityTypeId { get; set; }

		public string Security { get; set; }

		[Required(ErrorMessage = "Security is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Security is required")]
		public int SecurityId { get; set; }
				
		[Required(ErrorMessage = "Deal is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
		public int DealId { get; set; }

		[Required(ErrorMessage = "Record Date is required")]
		[DateRange()]
		public DateTime? RecordDate { get; set; }

		[Range((double)0, (double)decimal.MaxValue, ErrorMessage = "FMV is required")]
		public decimal? FMV { get; set; }

		[Range((double)0, (double)100, ErrorMessage = "Percent must be under 100%.")]
		public decimal? Percent { get; set; }

		[Range((int)0, int.MaxValue, ErrorMessage = "NumberOfShares is required")]
		public int? NumberOfShares { get; set; }

		public List<SelectListItem> Equities { get; set; }

		public List<SelectListItem> FixedIncomes { get; set; }
	}
}