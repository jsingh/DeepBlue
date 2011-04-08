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

		public CustomFieldValue(ICustomFieldValueService customFieldValueservice)
			: this() {
			this.customFieldValueservice = customFieldValueservice;
		}

		public CustomFieldValue() {
		}

		private ICustomFieldValueService _customFieldValueService;
		public ICustomFieldValueService customFieldValueservice {
			get {
				if (_customFieldValueService == null) {
					_customFieldValueService = new CustomFieldValueService();
				}
				return _customFieldValueService;
			}
			set {
				_customFieldValueService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var customFieldValue = this;
			IEnumerable<ErrorInfo> errors = Validate(customFieldValue);
			if (errors.Any()) {
				return errors;
			}
			customFieldValueservice.SaveCustomFieldValue(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CustomFieldValue customFieldValue) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(customFieldValue);
			return errors;
		}
	}
}