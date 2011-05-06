using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace DeepBlue.Models.Deal {
	 
	public class DealUnderlyingDirectListModel {

		public int DealUnderlyingDirectId { get; set; }

		public string DealName { get; set; }

		public DateTime CloseDate { get; set; }

		public string IssuerName { get; set; }

		public string SecurityType { get; set; }
	}
}