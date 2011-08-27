using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditFundClosingModel {
		public EditFundClosingModel() {
		}

		public int FundClosingId { get; set; }

		[Required(ErrorMessage = "Fund is required.")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		[DisplayName("Fund")]
		public int FundId { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		[StringLength(50, ErrorMessage = "Name must be under 50 characters.")]
		[DisplayName("Name")]
		public string Name { get; set; }

		public string FundName { get; set; }

		[Required(ErrorMessage = "Closing Date is required.")]
		[DisplayName("Closing Date")]
		public DateTime? FundClosingDate { get; set; }

		[DisplayName("First Closing")]
		public bool IsFirstClosing { get; set; }

		public List<SelectListItem> FundNames { get; set; }

	}
}