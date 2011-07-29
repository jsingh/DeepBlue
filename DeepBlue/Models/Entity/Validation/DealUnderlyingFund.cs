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

		private IDealUnderlyingFundService _dealUnderlyingFundService;
		public IDealUnderlyingFundService DealUnderlyingFundService {
			get {
				if (_dealUnderlyingFundService == null) {
					_dealUnderlyingFundService = new DealUnderlyingFundService();
				}
				return _dealUnderlyingFundService;
			}
			set {
				_dealUnderlyingFundService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var dealUnderlyingFund = this;
			IEnumerable<ErrorInfo> errors = Validate(dealUnderlyingFund);
			if (errors.Any()) {
				return errors;
			}
			DealUnderlyingFundService.SaveDealUnderlyingFund(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DealUnderlyingFund dealUnderlyingFund) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(dealUnderlyingFund);
			return errors;
		}
	}
}