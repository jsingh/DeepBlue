using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity.Partial;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingFundCapitalCallMD))]
	public partial class UnderlyingFundCapitalCall {
		public class UnderlyingFundCapitalCallMD : CreatedByFields {
			#region Primitive Properties

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

			[Required(ErrorMessage = "Due Date is required")]
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

		public UnderlyingFundCapitalCall(IUnderlyingFundCapitalCallService underlyingFundCapitalCallService)
			: this() {
				this.UnderlyingFundCapitalCallService = underlyingFundCapitalCallService;
		}

		public UnderlyingFundCapitalCall() {
		}

		private IUnderlyingFundCapitalCallService _UnderlyingFundCapitalCallService;
		public IUnderlyingFundCapitalCallService UnderlyingFundCapitalCallService {
			get {
				if (_UnderlyingFundCapitalCallService == null) {
					_UnderlyingFundCapitalCallService = new UnderlyingFundCapitalCallService();
				}
				return _UnderlyingFundCapitalCallService;
			}
			set {
				_UnderlyingFundCapitalCallService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingFundCapitalCallService.SaveUnderlyingFundCapitalCall(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFundCapitalCall underlyingFundCapitalCall) {
			return ValidationHelper.Validate(underlyingFundCapitalCall);
		}
	}
}