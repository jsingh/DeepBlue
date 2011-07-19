using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {
	public class CreateDistributionModel {
		
		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
		[DisplayName("Fund-")]
		public int FundId { get; set; }

		[Required(ErrorMessage = "Distribution number is required")]
		[StringLength(12)]
		[DisplayName("Distribution #-")]
		public string DistributionNumber { get; set; }

		[Required(ErrorMessage = "Distribution Amount is required")]
		[Range(typeof(decimal),"1", "79228162514264337593543950335", ErrorMessage = "Distribution Amount is required")]
		[DisplayName("Distribution Amount-")]
		public decimal DistributionAmount { get; set; }
		
		[Required(ErrorMessage = "Distribution Date is required")]
		[DateRange()]
		[DisplayName("Distribution Date-")]
		public DateTime CapitalDistributionDate { get; set; }
		
		[Required(ErrorMessage = "Distribution Due Date is required")]
        [DateRange()]
		[DisplayName("Distribution Due Date-")]
		public DateTime CapitalDistributionDueDate { get; set; }

		[DisplayName("Preferred Return-")]
		public decimal? PreferredReturn { get; set; }

		[DisplayName("Return Management Fees-")]
		public decimal? ReturnManagementFees { get; set; }

		[DisplayName("Return Fund Expenses-")]
		public decimal? ReturnFundExpenses { get; set; }

		[DisplayName("Preferred Catch Up-")]
		public decimal? PreferredCatchUp { get; set; }

		[DisplayName("GP Profits-")]
		public decimal? GPProfits { get; set; }

		[DisplayName("LP Profits-")]
		public decimal? LPProfits { get; set; }

		public int? InvestorCount { get; set; }
	 
	}
}