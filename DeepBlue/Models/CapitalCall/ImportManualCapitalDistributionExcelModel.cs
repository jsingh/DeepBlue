using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Deal;

namespace DeepBlue.Models.CapitalCall {

	public class ImportManualCapitalDistributionExcelModel : ImportExcelPagingModel {

		public string ManualCapitalDistributionTableName { get; set; }

		public string InvestorName { get; set; }

		public string FundName { get; set; }

		public string CapitalDistributionAmount { get; set; }
		
		public string ReturnManagementFees { get; set; }
		
		public string ReturnFundExpenses { get; set; }
		
		public string CostReturned { get; set; }
		
		public string Profits { get; set; }
		
		public string ProfitsReturned { get; set; }
		
		public string DistributionDate { get; set; }

		public string DistributionDueDate { get; set; }

	}
}