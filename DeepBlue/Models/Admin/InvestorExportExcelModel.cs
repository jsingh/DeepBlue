using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Admin {
	public class InvestorExportExcelModel {
	
		public object Investors { get; set; }
		
		public object InvestorAddresses { get; set; }

		public object InvestorBanks { get; set; }

		public object InvestorContacts { get; set; }

		public object InvestorInvestments { get; set; }
	}
	 
}