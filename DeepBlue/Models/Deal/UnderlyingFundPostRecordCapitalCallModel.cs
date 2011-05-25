using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class UnderlyingFundPostRecordCapitalCallModel {

		public int UnderlyingFundCapitalCallLineItemId { get; set; }

		[Required(ErrorMessage = "Underlying Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Underlying Fund is required")]
		public int UnderlyingFundId { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		public string FundName { get; set; }

		[Required(ErrorMessage = "Deal is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
		public int DealId { get; set; }

		public string DealName { get; set; }
		
		[Required(ErrorMessage = "Capital Call Amount is required")]
		[Range((double)1, (double)decimal.MaxValue, ErrorMessage = "Capital Call Amount is required")]
		public decimal? Amount { get; set; }

		[Required(ErrorMessage = "Received Date is required")]
		[DateRange()]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime? ReceivedDate { get; set; }
	}
}