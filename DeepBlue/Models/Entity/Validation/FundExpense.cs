using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(FundExpenseMD))]
	public partial class FundExpense {
		public class FundExpenseMD : CreatedByFields {
			#region Primitive Properties

			[Required(ErrorMessage = "Fund is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required(ErrorMessage = "Fund Expense Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund Expense Type is required")]
			public global::System.Int32 FundExpenseTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "Expense Amount is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Expense Amount is required")]
			public global::System.Decimal Amount {
				get;
				set;
			}

			#endregion
		}

		public FundExpense(IFundExpenseService fundExpenseService)
			: this() {
			this.FundExpenseService = fundExpenseService;
		}

		public FundExpense() {
		}

		private IFundExpenseService _FundExpenseService;
		public IFundExpenseService FundExpenseService {
			get {
				if (_FundExpenseService == null) {
					_FundExpenseService = new FundExpenseService();
				}
				return _FundExpenseService;
			}
			set {
				_FundExpenseService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			FundExpenseService.SaveFundExpense(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(FundExpense fundExpense) {
			return ValidationHelper.Validate(fundExpense);
		}
	}
}