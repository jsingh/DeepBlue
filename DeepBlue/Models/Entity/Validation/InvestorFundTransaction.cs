using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(InvestorFundTransactionMD))]
	public partial class InvestorFundTransaction {
		public class InvestorFundTransactionMD : CreatedByFields {

			#region Primitive Properties
			[Required(ErrorMessage = "Fund Closing is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund Closing is required")]
			public global::System.Int32 FundClosingID {
				get;
				set;
			}

			[Required(ErrorMessage = "Other Investor is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Other Investor is required")]
			public global::System.Int32 OtherInvestorID {
				get;
				set;
			}

			[Required(ErrorMessage = "Transaction Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Transaction Type is required")]
			public global::System.Int32 TransactionTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "Amount is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Amount is required")]
			public global::System.Decimal Amount {
				get;
				set;
			}

			#endregion
		}

		public InvestorFundTransaction(IInvestorFundTransactionService investorFundTransactionService)
			: this() {
			this.InvestorFundTransactionService = investorFundTransactionService;
		}

		public InvestorFundTransaction() {
		}

		private IInvestorFundTransactionService _InvestorFundTransactionService;
		public IInvestorFundTransactionService InvestorFundTransactionService {
			get {
				if (_InvestorFundTransactionService == null) {
					_InvestorFundTransactionService = new InvestorFundTransactionService();
				}
				return _InvestorFundTransactionService;
			}
			set {
				_InvestorFundTransactionService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(this);
			if (errors.Any()) {
				return errors;
			}
			InvestorFundTransactionService.SaveInvestorFundTransaction(this);
			return null;
		}


		private IEnumerable<ErrorInfo> Validate(InvestorFundTransaction investorFundTransaction) {
			return ValidationHelper.Validate(investorFundTransaction);
		}
	}


}