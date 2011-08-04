using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(SecurityConversionMD))]
	public partial class SecurityConversion {
		public class SecurityConversionMD : CreatedByFields {
			#region Primitive Properties

			[Required(ErrorMessage = "OldSecurityTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "OldSecurityTypeID is required")]
			public global::System.Int32 OldSecurityTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "OldSecurityID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "OldSecurityID is required")]
			public global::System.Int32 OldSecurityID {
				get;
				set;
			}

			[Required(ErrorMessage = "NewSecurityTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "NewSecurityTypeID is required")]
			public global::System.Int32 NewSecurityTypeID {
				get;
				set;
			}


			[Required(ErrorMessage = "NewSecurityID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "NewSecurityID is required")]
			public global::System.Int32 NewSecurityID {
				get;
				set;
			}


			[Required(ErrorMessage = "SplitFactor is required")]
			[Range((int)1, int.MaxValue, ErrorMessage = "SplitFactor is required")]
			public global::System.Int32 SplitFactor {
				get;
				set;
			}

			[Required(ErrorMessage = "ConversionDate is required")]
			[DateRange()]
			public global::System.DateTime ConversionDate {
				get;
				set;
			}
			#endregion
		}

		public SecurityConversion(ISecurityConversionService securityConversionService)
			: this() {
			this.SecurityConversionService = securityConversionService;
		}

		public SecurityConversion() {
		}

		private ISecurityConversionService _SecurityConversionService;
		public ISecurityConversionService SecurityConversionService {
			get {
				if (_SecurityConversionService == null) {
					_SecurityConversionService = new SecurityConversionService();
				}
				return _SecurityConversionService;
			}
			set {
				_SecurityConversionService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			SecurityConversionService.SaveSecurityConversion(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(SecurityConversion securityConversion) {
			return ValidationHelper.Validate(securityConversion);
		}
	}
}