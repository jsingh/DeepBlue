using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingFundNAVMD))]
	public partial class UnderlyingFundNAV {
		public class UnderlyingFundNAVMD {
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

			[Required(ErrorMessage = "FundNAV is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "FundNAV is required")]
			public global::System.Decimal FundNAV {
				get;
				set;
			}

			[Required(ErrorMessage = "FundNAVDate is required")]
			[DateRange()]
			public global::System.DateTime FundNAVDate {
				get;
				set;
			}
			#endregion
		}

		public UnderlyingFundNAV(IUnderlyingFundNAVService underlyingFundNAVService)
			: this() {
			this.UnderlyingFundNAVService = underlyingFundNAVService;
		}

		public UnderlyingFundNAV() {
		}

		private IUnderlyingFundNAVService _underlyingFundNAVService;
		public IUnderlyingFundNAVService UnderlyingFundNAVService {
			get {
				if (_underlyingFundNAVService == null) {
					_underlyingFundNAVService = new UnderlyingFundNAVService();
				}
				return _underlyingFundNAVService;
			}
			set {
				_underlyingFundNAVService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var underlyingFundNAV = this;
			IEnumerable<ErrorInfo> errors = Validate(underlyingFundNAV);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingFundNAVService.SaveUnderlyingFundNAV(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFundNAV underlyingFundNAV) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(underlyingFundNAV);
			return errors;
		}
	}
}