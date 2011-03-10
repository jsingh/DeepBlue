using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Fund {
	public class FundDetail {
		public FundDetail() {
			FundId = 0;
			AccountId = 0;
		}

		public int FundId { get; set; }

		public int AccountId { get; set; }

		[Required(ErrorMessage = "Fund Name is required")]
		[DisplayName("Fund Name:")]
		public string FundName { get; set; }

		[Required(ErrorMessage = "Tax Id is required")]
		[RemoteUID_(Action = "TaxIdAvailable", Controller = "Fund", ValidateParameterName = "TaxId", Params = new string[] {"FundId"})]
		[DisplayName("Tax Id:")]
		public string TaxId { get; set; }

		[Required(ErrorMessage = "Fund Start Date is required")]
		[DisplayName("Fund Start Date:")]
		public DateTime FundStartDate { get; set; }

		[DisplayName("Schedule Termination Date:")]
		public DateTime? ScheduleTerminationDate { get; set; }

		[DisplayName("Final Termination Date:")]
		public DateTime? FinalTerminationDate { get; set; }

		[DisplayName("Num of Automatic Extensions:")]
		public int? NumofAutoExtensions { get; set; }

		[DisplayName("Date Clawback Triggered:")]
		public DateTime? DateClawbackTriggered { get; set; }

		[DisplayName("Recycle Provision %:")]
		public int? RecycleProvision { get; set; }

		[DisplayName("Management Fees Catch Up Date:")]
		public DateTime? MgmtFeesCatchUpDate { get; set; }

		[DisplayName("Carry %:")]
		public int? Carry { get; set; }

		/* Bank Details */

		[Required(ErrorMessage = "Bank Name is required")]
		[DisplayName("Bank Name:")]
		public string BankName { get; set; }

		[Required(ErrorMessage = "Account is required")]
		[DisplayName("Account:")]
		public string Account { get; set; }

		[DisplayName("ABA Number:")]
		public string ABANumber { get; set; }

		[DisplayName("Swift Code:")]
		public string Swift { get; set; }

		[DisplayName("Account Number Cash:")]
		public string AccountNumberCash { get; set; }

		[DisplayName("FFC Number:")]
		public string FFCNumber { get; set; }

		[DisplayName("IBAN:")]
		public string IBAN { get; set; }

		[DisplayName("Reference:")]
		public string Reference { get; set; }

		[DisplayName("Account Of:")]
		public string AccountOf { get; set; }

		[DisplayName("Attention:")]
		public string Attention { get; set; }

		[DisplayName("Telephone:")]
		public string Telephone { get; set; }

		[DisplayName("Fax:")]
		public string Fax { get; set; }
	}
}