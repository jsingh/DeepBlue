using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class CreateDealCloseModel : FinalDealCloseModel {

		public CreateDealCloseModel() {
			DealUnderlyingFunds = new List<DealUnderlyingFundModel>();
			DealUnderlyingDirects = new List<DealUnderlyingDirectModel>();
			CloseDate = DateTime.Now;
		}

		public int DealClosingId { get; set; }

		[Required(ErrorMessage = "Deal is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
		public int DealId { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }
				
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal Close Number is required")]
		[DisplayName("Deal Close #:")]
		public int? DealNumber { get; set; }

		[Required(ErrorMessage = "Close Date is required")]
		[DateRange()]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DisplayName("Close Date:")]
		public DateTime CloseDate { get; set; }

		[DisplayName("Final Close:")]
		public bool IsFinalClose { get; set; }

        public string DealName { get; set; }

	}

}