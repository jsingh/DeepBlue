using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Admin {
	public class UnderlyingFundExportExcelModel {

		public object UnderlyingFunds { get; set; }

		public object UnderlyingFundContacts { get; set; }

		public object UnderlyingFundCapitalCalls { get; set; }

		public object UnderlyingFundCapitalCallLineItems { get; set; }

		public object UnderlyingFundCashDistributions { get; set; }

		public object UnderlyingFundCashDistributionLineItems { get; set; }

		public object UnderlyingFundStockDistributions { get; set; }

		public object UnderlyingFundStockDistributionLineItems { get; set; }
	}
}