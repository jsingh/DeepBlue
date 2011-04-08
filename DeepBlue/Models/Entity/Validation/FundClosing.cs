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

		public FundClosing(IFundClosingService fundservice)
			: this() {
			this.fundcloseservice = fundservice;
		}

		public FundClosing() {
		}

		private IFundClosingService _fundService;
		public IFundClosingService fundcloseservice {
			get {
				if (_fundService == null) {
					_fundService = new FundClosingService();
				}
				return _fundService;
			}
			set {
				_fundService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var fundclose = this;
			IEnumerable<ErrorInfo> errors = Validate(fundclose);
			if (errors.Any()) {
				return errors;
			}
			fundcloseservice.SaveFundClose(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(FundClosing fundclosing) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(fundclosing);
			return errors;
		}
	}
}