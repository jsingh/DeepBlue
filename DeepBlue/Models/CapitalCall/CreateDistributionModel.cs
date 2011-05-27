using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.CapitalCall {
	public class CreateDistributionModel {

		[DisplayName("Fund:")]
		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
		public int FundId { get; set; }

		[DisplayName("Distribution #:")]
		[Required(ErrorMessage = "Distribution number is required")]
		[StringLength(12)]
		public string DistributionNumber { get; set; }

		[DisplayName("Distribution Amount:")]
		[Required(ErrorMessage = "Distribution Amount is required")]
		[Range(typeof(decimal),"1", "79228162514264337593543950335", ErrorMessage = "Distribution Amount is required")]
		public decimal DistributionAmount { get; set; }

		[DisplayName("Distribution Date:")]
		[Required(ErrorMessage = "Distribution Date is required")]
		[DateRange()]
		public DateTime CapitalDistributionDate { get; set; }

		[DisplayName("Distribution Due Date:")]
		[Required(ErrorMessage = "Distribution Due Date is required")]
        [DateRange()]
		public DateTime CapitalDistributionDueDate { get; set; }

		[DisplayName("Add Preferred Return")]
		public bool AddPreferredReturn { get; set; }

		[DisplayName("Add Return Management Fees")]
		public bool AddReturnManagementFees { get; set; }

		[DisplayName("Add Return Fund Expenses")]
		public bool AddReturnFundExpenses { get; set; }

		[DisplayName("Add Preferred Catch Up")]
		public bool AddPreferredCatchUp { get; set; }

		[DisplayName("Profits")]
		public bool AddProfits { get; set; }

		public decimal? PreferredReturn { get; set; }

		public decimal? ReturnManagementFees { get; set; }

		public decimal? ReturnFundExpenses { get; set; }

		public decimal? PreferredCatchUp { get; set; }

		[DisplayName("GPProfits")]
		public decimal? GPProfits { get; set; }

		[DisplayName("LPProfits")]
		public decimal? LPProfits { get; set; }
	 
	}
}