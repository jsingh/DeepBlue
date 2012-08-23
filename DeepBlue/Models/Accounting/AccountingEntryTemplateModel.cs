using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DeepBlue.Models.Accounting {
	public class AccountingEntryTemplateModel {

		#region Primitive Properties
	 
		public Int32 AccountingEntryTemplateID { get; set; }

		[Required(ErrorMessage = "Fund is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
		[Display(Name = "Fund")]
		public Int32 FundID {
			get;
			set;
		}

		[Display(Name = "Fund")]
		public string FundName { get; set; }

		[Required(ErrorMessage = "Transaction Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Transaction Type is required")]
		[Display(Name = "Transaction Type")]
		public Int32 AccountingTransactionTypeID { get; set; }

		[Display(Name = "Transaction Type")]
		public string AccountingTransactionTypeName { get; set; }

		[Display(Name = "Credit")]
		public Boolean IsCredit { get; set; }

		[Required(ErrorMessage = "Virtual Account is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Virtual Account is required")]
		[Display(Name = "Virtual Account")]
		public Int32 VirtualAccountID { get; set; }

		[Display(Name = "Virtual Account")]
		public string VirtualAccountName { get; set; }

		[StringLength(100, ErrorMessage = "Description must be under 100 characters.")]
		[Display(Name = "Description")]
		public String Description {
			get;
			set;
		}

		[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Amount is required")]
		[Display(Name = "Amount")]
		public Decimal? Amount {
			get;
			set;
		}

		[Required(ErrorMessage = "Amount Type is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Amount Type is required")]
		[Display(Name = "Amount Type")]
		public Int32 AccountingEntryAmountTypeID { get; set; }

				[Display(Name = "Amount Type")]
		public string AccountingEntryAmountTypeName { get; set; }

		[StringLength(50, ErrorMessage = "Percentage must be under 50 characters.")]
		[Display(Name = "Percentage")]
		public String AccountingEntryAmountTypeData {
			get;
			set;
		}

		#endregion
	 

	}
}