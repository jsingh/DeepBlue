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
		public class InvestorFundMD : CreatedByFields {

			#region Primitive Properties
			[Required(ErrorMessage = "Fund is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			#endregion
		}

		public InvestorFund(IInvestorFundService investorFundService)
			: this() {
			this.InvestorFundService = investorFundService;
		}

		public InvestorFund() {

		}

		private IInvestorFundService _InvestorFundService;
		public IInvestorFundService InvestorFundService {
			get {
				if (_InvestorFundService == null) {
					_InvestorFundService = new InvestorFundService();
				}
				return _InvestorFundService;
			}
			set {
				_InvestorFundService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			InvestorFundService.SaveInvestorFund(this);
			return null;
		}


		private IEnumerable<ErrorInfo> Validate(InvestorFund investorFund) {
			return ValidationHelper.Validate(investorFund);
		}

	}


}