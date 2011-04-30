using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Issuer {
	public class FixedIncomeDetailModel {

		public string IssuerName { get; set; }

		public int IssuerId { get; set; }

		public int FixedIncomeId { get; set; }

		public string Symbol { get; set; }
	}
}