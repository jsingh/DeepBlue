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