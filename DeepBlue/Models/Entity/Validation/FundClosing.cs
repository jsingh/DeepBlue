using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(FundClosingMD))]
	public partial class FundClosing {
		public class FundClosingMD {
			#region Primitive Properties
			[Required(ErrorMessage = "Fund is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required(ErrorMessage = "Fund Closing Date is required")]
			[DateRange(ErrorMessage = "Fund Closing Date is required")]
			public global::System.DateTime FundClosingDate {
				get;
				set;
			}

			[Required(ErrorMessage = "Name is required")]
			[StringLength(50, ErrorMessage = "Name must be under 50 characters.")]
			public global::System.String Name {
				get;
				set;
			}
			#endregion
		}

		public FundClosing(IFundClosingService fundClosingService)
			: this() {
				this.FundClosingService = fundClosingService;
		}

		public FundClosing() {
		}

		private IFundClosingService _FundClosingService;
		public IFundClosingService FundClosingService {
			get {
				if (_FundClosingService == null) {
					_FundClosingService = new FundClosingService();
				}
				return _FundClosingService;
			}
			set {
				_FundClosingService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			FundClosingService.SaveFundClose(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(FundClosing fundclosing) {
			return ValidationHelper.Validate(fundclosing);
		}
	}
}