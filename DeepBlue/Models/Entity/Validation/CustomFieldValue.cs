using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CustomFieldValueMD))]
	public partial class CustomFieldValue {
		public class CustomFieldValueMD {
			#region Primitive Properties
			[Required(ErrorMessage = "Custom Field is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Custom Field is required")]
			public global::System.Int32 CustomFieldID {
				get;
				set;
			}

			[Required(ErrorMessage = "Key is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Key is required")]
			public global::System.Int32 Key {
				get;
				set;
			}

			#endregion
		}

		public CustomFieldValue(ICustomFieldValueService customFieldValueService)
			: this() {
				this.CustomFieldValueService = customFieldValueService;
		}

		public CustomFieldValue() {
		}

		private ICustomFieldValueService _CustomFieldValueService;
		public ICustomFieldValueService CustomFieldValueService {
			get {
				if (_CustomFieldValueService == null) {
					_CustomFieldValueService = new CustomFieldValueService();
				}
				return _CustomFieldValueService;
			}
			set {
				_CustomFieldValueService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			CustomFieldValueService.SaveCustomFieldValue(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CustomFieldValue customFieldValue) {
			return ValidationHelper.Validate(customFieldValue);
		}
	}
}