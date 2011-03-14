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
			[Required]
			[Range((int)(int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required]
			[Range(typeof(DateTime), "1/1/1900", "1/1/9999")]
			public global::System.DateTime FundClosingDate {
				get;
				set;
			}

			[StringLength(50), Required]
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