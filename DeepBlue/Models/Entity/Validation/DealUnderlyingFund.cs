using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(DealUnderlyingFundMD))]
	public partial class DealUnderlyingFund {
		public class DealUnderlyingFundMD {
			#region Primitive Properties
			[Required(ErrorMessage = "UnderlyingFundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "UnderlyingFundID is required")]
			public global::System.Int32 UnderlyingFundID {
				get;
				set;
			}

			[Required(ErrorMessage = "Deal is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
			public global::System.Int32 DealID {
				get;
				set;
			}

			[Required(ErrorMessage = "RecordDate is required")]
			[DateRange()]
			public global::System.DateTime RecordDate {
				get;
				set;
			}

            [Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "CommittedAmount is required")]
			public global::System.Decimal CommittedAmount {
				get;
				set;
			}
			#endregion
		}

		public DealUnderlyingFund(IDealUnderlyingFundService dealUnderlyingFundService)
			: this() {
			this.DealUnderlyingFundService = dealUnderlyingFundService;
		}

		public DealUnderlyingFund() {
		}

		private IDealUnderlyingFundService _DealUnderlyingFundService;
		public IDealUnderlyingFundService DealUnderlyingFundService {
			get {
				if (_DealUnderlyingFundService == null) {
					_DealUnderlyingFundService = new DealUnderlyingFundService();
				}
				return _DealUnderlyingFundService;
			}
			set {
				_DealUnderlyingFundService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			DealUnderlyingFundService.SaveDealUnderlyingFund(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DealUnderlyingFund dealUnderlyingFund) {
			return ValidationHelper.Validate(dealUnderlyingFund);
		}
	}
}