using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Issuer {
	public class EquityDetailModel {

		public string IssuerName { get; set; }

		public int IssuerId { get; set; }

		public int EquityId { get; set; }

		public string Symbol { get; set; }
	}
}