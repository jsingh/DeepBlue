using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CustomFieldMD))]
	public partial class CustomField {
		public class CustomFieldMD {
			#region Primitive Properties
			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 ModuleId {
				get;
				set;
			}

			[Required]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue)]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 DataTypeID {
				get;
				set;
			}

			[StringLength(50), Required]
			public global::System.String CustomFieldText {
				get;
				set;
			}
			#endregion
		}

		public CustomField(ICustomFieldService customFieldservice)
			: this() {
			this.customFieldservice = customFieldservice;
		}

		public CustomField() {
		}

		private ICustomFieldService _customFieldService;
		public ICustomFieldService customFieldservice {
			get {
				if (_customFieldService == null) {
					_customFieldService = new CustomFieldService();
				}
				return _customFieldService;
			}
			set {
				_customFieldService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var customField = this;
			IEnumerable<ErrorInfo> errors = Validate(customField);
			if (errors.Any()) {
				return errors;
			}
			customFieldservice.SaveCustomField(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CustomField customFieldclosing) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(customFieldclosing);
			return errors;
		}
	}
}