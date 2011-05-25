using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity.Partial;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingFundCashDistributionMD))]
	public partial class UnderlyingFundCashDistribution {
		public class UnderlyingFundCashDistributionMD {
			#region Primitive Properties

			[Required(ErrorMessage = "Cash Distribution Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Cash Distribution Type is required")]
			public global::System.Int32 CashDistributionTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "Fund is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required(ErrorMessage = "Underlying Fund is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Underlying Fund is required")]
			public global::System.Int32 UnderlyingFundID {
				get;
				set;
			}

			[Required(ErrorMessage = "Amount is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Amount is required")]
			public global::System.Decimal Amount {
				get;
				set;
			}

			[Required(ErrorMessage = "Notice Date is required")]
			[DateRange()]
			public global::System.DateTime NoticeDate {
				get;
				set;
			}
			 
			[Required(ErrorMessage = "Received Date is required")]
			[DateRange()]
			public global::System.DateTime ReceivedDate {
				get;
				set;
			}
			#endregion
		}

		public UnderlyingFundCashDistribution(IUnderlyingFundCashDistributionService underlyingFundCashDistributionservice)
			: this() {
			this.UnderlyingFundCashDistributionService = UnderlyingFundCashDistributionService;
		}

		public UnderlyingFundCashDistribution() {
		}

		private IUnderlyingFundCashDistributionService _UnderlyingFundCashDistributionService;
		public IUnderlyingFundCashDistributionService UnderlyingFundCashDistributionService {
			get {
				if (_UnderlyingFundCashDistributionService == null) {
					_UnderlyingFundCashDistributionService = new UnderlyingFundCashDistributionService();
				}
				return _UnderlyingFundCashDistributionService;
			}
			set {
				_UnderlyingFundCashDistributionService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var underlyingFundCashDistribution = this;
			IEnumerable<ErrorInfo> errors = Validate(underlyingFundCashDistribution);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingFundCashDistributionService.SaveUnderlyingFundCashDistribution(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFundCashDistribution underlyingFundCashDistribution) {
			return ValidationHelper.Validate(underlyingFundCashDistribution);
		}
	}
}