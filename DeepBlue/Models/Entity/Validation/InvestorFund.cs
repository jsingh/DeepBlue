using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(InvestorFundMD))]
	public partial class InvestorFund {
		public class InvestorFundMD {

			#region Primitive Properties
			[Required(ErrorMessage = "Fund is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required(ErrorMessage = "CreatedBy is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CreatedBy is required")]
			public global::System.Int32 CreatedBy {
				get;
				set;
			}

			[Required(ErrorMessage = "Created Date is required")]
			[DateRange(ErrorMessage = "Created Date is required")]
			public global::System.DateTime CreatedDate {
				get;
				set;
			}

			[Required(ErrorMessage = "Committed Date is required")]
			[DateRange(ErrorMessage = "Committed Date is required")]
			public global::System.DateTime CommittedDate {
				get;
				set;
			}

			[Required(ErrorMessage="Total Commitment is required")]
			[Range(1, (double)decimal.MaxValue, ErrorMessage = "Total Commitment is required")]
			public global::System.Decimal TotalCommitment {
				get;
				set;
			}
			#endregion
		}

		public InvestorFund(IInvestorFundService investorService)
			: this() {
			this.InvestorFundService = investorService;
		}

		public InvestorFund() {

		}

		private IInvestorFundService _investorService;
		public IInvestorFundService InvestorFundService {
			get {
				if (_investorService == null) {
					_investorService = new InvestorFundService();
				}
				return _investorService;
			}
			set {
				_investorService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var investorFund = this;
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(investorFund);
			if (errors.Any()) {
				return errors;
			}
			InvestorFundService.SaveInvestorFund(this);
			return null;
		}

	}


}