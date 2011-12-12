using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingFundNAVMD))]
	public partial class UnderlyingFundNAV {
		public class UnderlyingFundNAVMD : CreatedByFields {
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
			[Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "FundNAV is required")]
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

		private IUnderlyingFundNAVService _UnderlyingFundNAVService;
		public IUnderlyingFundNAVService UnderlyingFundNAVService {
			get {
				if (_UnderlyingFundNAVService == null) {
					_UnderlyingFundNAVService = new UnderlyingFundNAVService();
				}
				return _UnderlyingFundNAVService;
			}
			set {
				_UnderlyingFundNAVService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingFundNAVService.SaveUnderlyingFundNAV(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFundNAV underlyingFundNAV) {
			return ValidationHelper.Validate(underlyingFundNAV);
		}
	}
}