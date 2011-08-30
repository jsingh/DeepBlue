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
		public class InvestorFundTransactionMD  {

			#region Primitive Properties

			[Required(ErrorMessage = "Investor Fund is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Investor Fund is required")]
			public global::System.Int32 InvestorFundID {
				get;
				set;
			}

			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Amount is required")]
			public Nullable<global::System.Decimal> Amount {
				get;
				set;
			}

			[Required(ErrorMessage = "TransactionTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Transaction Type is required")]
			public global::System.Int32 TransactionTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "IsAgreementSigned is required")]
			public global::System.Boolean IsAgreementSigned {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund Closing is required")]
			public Nullable<global::System.Int32> FundClosingID {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "OtherInvestorID is required")]
			public Nullable<global::System.Int32> OtherInvestorID {
				get;
				set;
			}
			 
			[Required(ErrorMessage = "CreatedDate is required")]
			[DateRange(ErrorMessage = "CreatedDate is required")]
			public global::System.DateTime CreatedDate {
				get;
				set;
			}

			[Required(ErrorMessage = "CreatedBy is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CreatedBy is required")]
			public global::System.Int32 CreatedBy {
				get;
				set;
			}

			[DateRange(ErrorMessage = "CommittedDate is required")]
			public Nullable<global::System.DateTime> CommittedDate {
				get;
				set;
			}

			[StringLength(500, ErrorMessage = "Notes must be under 500 characters.")]
			public global::System.String Notes {
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