using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DeepBlue.Models.Accounting {
	public class VirtualAccountModel {

		public int VirtualAccountID { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		public int FundID { get; set; }

		[DisplayName("Fund Name")]
		public string FundName { get; set; }

		[Required(ErrorMessage = "Account Name is required")]
		[StringLength(50, ErrorMessage = "Account Name must be under 50 characters.")]
		[DisplayName("Account Name")]
		public string AccountName { get; set; }

		public int? ParentVirtualAccountID { get; set; }

		[DisplayName("Parent Account")]
		public string ParentVirtualAccountName { get; set; }

		public int? ActualAccountID { get; set; }

		[DisplayName("Actual Account")]
		public string ActualAccountName { get; set; }

		[Required(ErrorMessage = "Ledger Balance is required")]
		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Ledger Balance is required")]
		[DisplayName("Ledger Balance")]
		public decimal LedgerBalance { get; set; }

	}
}