using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Deal {
	public class ImportUnderlyingFundValuationModel {

		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "PageIndex is required")]
		public int PageIndex { get; set; }

		[Required(ErrorMessage = "Underlying Fund is required")]
		public string UnderlyingFund { get; set; }

		[Required(ErrorMessage = "Amberbrook Fund is required")]
		public string AmberbrookFund { get; set; }

		[Required(ErrorMessage = "Update NAV is required")]
		public string UpdateNAV { get; set; }

		[Required(ErrorMessage = "Update Date is required")]
		public string UpdateDate { get; set; }

	}
}