using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {

	public class UnderlyingFundActivityFields {

		public UnderlyingFundActivityFields() {
			UnderlyingFundId = 0;
			FundId = 0;
		}

		[DisplayName("Post Record Date Transaction:")]
		public bool? IsPostRecordDateTransaction { get; set; }

		[Required(ErrorMessage = "Amount is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Amount is required")]
		[DisplayName("Amount:")]
		public decimal? Amount { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		[DisplayName("Fund:")]
		public int FundId { get; set; }

		[Required(ErrorMessage = "Underlying Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Underlying Fund is required")]
		[DisplayName("Underlying Fund:")]
		public int UnderlyingFundId { get; set; }

		[Required(ErrorMessage = "Notice Date is required")]
		[DateRange()]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DisplayName("Notice Date:")]
		public DateTime? NoticeDate { get; set; }

		[Required(ErrorMessage = "Received Date is required")]
		[DateRange()]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DisplayName("Received Date:")]
		public DateTime? ReceivedDate { get; set; }

		public string FundName { get; set; }

		public string UnderlyingFundName { get; set; }
	}
}