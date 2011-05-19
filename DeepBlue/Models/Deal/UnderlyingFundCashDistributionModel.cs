using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {

	public class UnderlyingFundCashDistributionModel : UnderlyingFundActivityFields {

		[Range(0, int.MaxValue, ErrorMessage = "UnderlyingFundCashDistributionId is required")]
		public int? UnderlyingFundCashDistributionId { get; set; }

		[Required(ErrorMessage = "Cash Distribution Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Cash Distribution Type is required")]
		[DisplayName("Cash Distribution:")]
		public int CashDistributionTypeId { get; set; }

		[Required(ErrorMessage = "Paid Date is required")]
		[DateRange()]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DisplayName("Paid Date:")]
		public DateTime? PaidDate { get; set; }

		public List<SelectListItem> CashDistributionTypes { get; set; }

	}


	public class UnderlyingFundCashDistributionList : UnderlyingFundActivityFields {

		public int UnderlyingFundCashDistributionId { get; set; }

	}
}