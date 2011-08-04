using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CashDistributionMD))]
	public partial class CashDistribution {
		public class CashDistributionMD : CreatedByFields {
			#region Primitive Properties
			[Required(ErrorMessage = "UnderlyingFundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "UnderlyingFundID is required")]
			public global::System.Int32 UnderlyingFundID {
				get;
				set;
			}

			[Required(ErrorMessage = "DealID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "DealID is required")]
			public global::System.Int32 DealID {
				get;
				set;
			}
			#endregion
		}

		public CashDistribution(ICashDistributionService cashDistributionservice)
			: this() {
			this.CashDistributionService = cashDistributionservice;
		}

		public CashDistribution() {
		}

		private ICashDistributionService _CashDistributionService;
		public ICashDistributionService CashDistributionService {
			get {
				if (_CashDistributionService == null) {
					_CashDistributionService = new CashDistributionService();
				}
				return _CashDistributionService;
			}
			set {
				_CashDistributionService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var cashDistribution = this;
			IEnumerable<ErrorInfo> errors = Validate(cashDistribution);
			if (errors.Any()) {
				return errors;
			}
			CashDistributionService.SaveCashDistribution(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CashDistribution cashDistribution) {
			return ValidationHelper.Validate(cashDistribution);
		}
	}
}