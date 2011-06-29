using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(SecurityConversionDetailMD))]
	public partial class SecurityConversionDetail {
		public class SecurityConversionDetailMD {
			#region Primitive Properties

			[Required(ErrorMessage = "SecurityConversionID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "SecurityConversionID is required")]
			public global::System.Int32 SecurityConversionID {
				get;
				set;
			}

			[Required(ErrorMessage = "DealUnderlyingDirectID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "DealUnderlyingDirectID is required")]
			public global::System.Int32 DealUnderlyingDirectID {
				get;
				set;
			}

			[Required(ErrorMessage = "OldNumberOfShares is required")]
			[Range((int)1, int.MaxValue, ErrorMessage = "OldNumberOfShares is required")]
			public global::System.Int32 OldNumberOfShares {
				get;
				set;
			}

			[Required(ErrorMessage = "NewNumberOfShares is required")]
			[Range((int)1, int.MaxValue, ErrorMessage = "NewNumberOfShares is required")]
			public global::System.Int32 NewNumberOfShares {
				get;
				set;
			}

			[Required(ErrorMessage = "OldFMV is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "OldFMV is required")]
			public global::System.Decimal OldFMV {
				get;
				set;
			}

			[Required(ErrorMessage = "NewNumberOfShares is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "NewFMV is required")]
			public global::System.Decimal NewFMV {
				get;
				set;
			}
		 
			#endregion
		}

		public SecurityConversionDetail(ISecurityConversionDetailService securityConversionDetailService)
			: this() {
			this.SecurityConversionDetailService = securityConversionDetailService;
		}

		public SecurityConversionDetail() {
		}

		private ISecurityConversionDetailService _securityConversionDetailService;
		public ISecurityConversionDetailService SecurityConversionDetailService {
			get {
				if (_securityConversionDetailService == null) {
					_securityConversionDetailService = new SecurityConversionDetailService();
				}
				return _securityConversionDetailService;
			}
			set {
				_securityConversionDetailService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var securityConversionDetail = this;
			IEnumerable<ErrorInfo> errors = Validate(securityConversionDetail);
			if (errors.Any()) {
				return errors;
			}
			SecurityConversionDetailService.SaveSecurityConversionDetail(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(SecurityConversionDetail securityConversionDetail) {
			return ValidationHelper.Validate(securityConversionDetail);
		}
	}
}