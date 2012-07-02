using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Deal;

namespace DeepBlue.Models.CapitalCall {

	public class ImportManualCapitalCallExcelModel : ImportExcelPagingModel {

		public string ManualCapitalCallTableName { get; set; }

		public string InvestorName { get; set; }

		public string FundName { get; set; }

		public string CapitalCallAmount { get; set; }
		
		public string ManagementFeesInterest { get; set; }
		
		public string InvestedAmountInterest { get; set; }
		
		public string ManagementFees { get; set; }
		
		public string FundExpenses { get; set; }
		
		public string CapitalCallDate { get; set; }

		public string CapitalCallDueDate { get; set; }

	}
}