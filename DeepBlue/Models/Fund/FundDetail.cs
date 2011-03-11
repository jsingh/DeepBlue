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
		[StringLength(50, ErrorMessage = "Fund Name Must is under 50 characters.")]
		[DisplayName("Fund Name:")]
		public string FundName { get; set; }

		[Required(ErrorMessage = "Tax Id is required")]
		[StringLength(50, ErrorMessage = "Tax Id Must is under 50 characters.")]
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
		[StringLength(50, ErrorMessage = "Bank Name Must is under 50 characters.")]
		[DisplayName("Bank Name:")]
		public string BankName { get; set; }

		[Required(ErrorMessage = "Account is required")]
		[StringLength(40, ErrorMessage = "Account Must is under 40 characters.")]
		[DisplayName("Account:")]
		public string Account { get; set; }

		[DisplayName("ABA Number:")]
		public string ABANumber { get; set; }

		[StringLength(50, ErrorMessage = "Swift Code Must is under 50 characters.")]
		[DisplayName("Swift Code:")]
		public string Swift { get; set; }

		[StringLength(50, ErrorMessage = "Account Number Cash Must is under 50 characters.")]
		[DisplayName("Account Number Cash:")]
		public string AccountNumberCash { get; set; }

		[StringLength(50, ErrorMessage = "FFC Number Must is under 50 characters.")]
		[DisplayName("FFC Number:")]
		public string FFCNumber { get; set; }

		[StringLength(50, ErrorMessage = "IBAN Must is under 50 characters.")]
		[DisplayName("IBAN:")]
		public string IBAN { get; set; }

		[StringLength(50, ErrorMessage = "Reference Must is under 50 characters.")]
		[DisplayName("Reference:")]
		public string Reference { get; set; }

		[StringLength(50, ErrorMessage = "Account Of Must is under 50 characters.")]
		[DisplayName("Account Of:")]
		public string AccountOf { get; set; }

		[StringLength(50, ErrorMessage = "Attention Must is under 50 characters.")]
		[DisplayName("Attention:")]
		public string Attention { get; set; }

		[StringLength(50, ErrorMessage = "Telephone Must is under 50 characters.")]
		[DisplayName("Telephone:")]
		public string Telephone { get; set; }

		[StringLength(50, ErrorMessage = "Fax Must is under 50 characters.")]
		[DisplayName("Fax:")]
		public string Fax { get; set; }
	}
}