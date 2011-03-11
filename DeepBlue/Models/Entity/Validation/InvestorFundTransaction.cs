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
		public class InvestorFundTransactionMD {

			#region Primitive Properties
			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 FundClosingID {
				get;
				set;
			}
 
			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 CreatedBy {
				get;
				set;
			}

			[Required]
			[Range(typeof(DateTime), "1/1/1900", "1/1/9999")]
			public global::System.DateTime CreatedDate {
				get;
				set;
			}

			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 OtherInvestorID {
				get;
				set;
			}

			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 TransactionTypeID {
				get;
				set;
			}

			[Required]
			[Range(typeof(DateTime), "1/1/1900", "1/1/9999")]
			public global::System.DateTime Amount {
				get;
				set;
			}
		
			#endregion
		}

		public InvestorFundTransaction(IInvestorFundTransactionService investorService)
			: this() {
			this.InvestorFundTransactionService = investorService;
		}

		public InvestorFundTransaction() {

		}

		private IInvestorFundTransactionService _investorService;
		public IInvestorFundTransactionService InvestorFundTransactionService {
			get {
				if (_investorService == null) {
					_investorService = new InvestorFundTransactionService();
				}
				return _investorService;
			}
			set {
				_investorService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var investorFundTransaction = this;
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(investorFundTransaction);
			if (errors.Any()) {
				return errors;
			}
			InvestorFundTransactionService.SaveInvestorFundTransaction(this);
			return null;
		}
	}


}