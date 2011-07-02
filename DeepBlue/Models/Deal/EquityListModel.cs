using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Deal {
	public class EquityListModel {

		public int EquityId { get; set; }

		public string Symbol { get; set; }

		public string Industry { get; set; }

		public string EquityType { get; set; }
	}
}