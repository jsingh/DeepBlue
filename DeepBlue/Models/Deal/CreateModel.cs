using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Web.Mvc;


namespace DeepBlue.Models.Deal {
	public class CreateModel : DealDetailModel {

		public List<SelectListItem> Contacts { get; set; }

		public List<SelectListItem> PurchaseTypes { get; set; }

		public List<SelectListItem> DocumentTypes { get; set; }

		public List<SelectListItem> DealClosingCostTypes { get; set; }

		public List<SelectListItem> UnderlyingFunds { get; set; }

		public List<SelectListItem> Issuers { get; set; }

		public List<SelectListItem> SecurityTypes { get; set; }

		public List<SelectListItem> Securities { get; set; }
	}
}