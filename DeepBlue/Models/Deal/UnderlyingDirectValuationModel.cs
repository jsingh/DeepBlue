using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Deal {
	public class UnderlyingDirectValuationModel {

		public int UnderlyingDirectLastPriceId { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundId { get; set; }

		public string FundName { get; set; }
		
		[Required(ErrorMessage = "Security Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Security Type is required")]
		public int SecurityTypeId { get; set; }

		public string SecurityType { get; set; }

		[Required(ErrorMessage = "Security is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Security is required")]
		public int SecurityId { get; set; }

		public string Security { get; set; }

		public decimal? LastPrice { get; set; }
	
		public DateTime? LastPriceDate { get; set; }

		[Required(ErrorMessage = "New Price is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "New Price is required")]
		public decimal NewPrice { get; set; }

		[Required(ErrorMessage = "New Price Date is required")]
		[DateRange()]
		public DateTime NewPriceDate { get; set; }
		
	}
}