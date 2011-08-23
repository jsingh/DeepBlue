using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class EquitySplitModel : SecurityActivityModel {

		public EquitySplitModel() {
			SplitDate = DateTime.Now;
		}
	 
		[Required(ErrorMessage = "Direct is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Direct is required")]
		[DisplayName("Direct-")]
		public int EquityId { get; set; }
	 
		[DisplayName("Direct Type-")]
		public int SecurityTypeId { get; set; }

		[DisplayName("Direct Symbol-")]
		public string Symbol { get; set; }

		[Required(ErrorMessage = "Split Date is required")]
		[DateRange(ErrorMessage = "Split Date is required")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		[DisplayName("Split Date-")]
		public DateTime SplitDate { get; set; }

	}
}