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
			[Required(ErrorMessage = "Module is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Module is required")]
			public global::System.Int32 ModuleID {
				get;
				set;
			}

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "DataType is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "DataType is required")]
			public global::System.Int32 DataTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "Custom Field Text is required")]
			[StringLength(50, ErrorMessage = "Custom Field Text must be under 50 characters.")]
			public global::System.String CustomFieldText {
				get;
				set;
			}
			#endregion
		}

		public CustomField(ICustomFieldService customFieldService)
			: this() {
				this.CustomFieldservice = customFieldService;
		}

		public CustomField() {
		}

		private ICustomFieldService _CustomFieldService;
		public ICustomFieldService CustomFieldservice {
			get {
				if (_CustomFieldService == null) {
					_CustomFieldService = new CustomFieldService();
				}
				return _CustomFieldService;
			}
			set {
				_CustomFieldService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			CustomFieldservice.SaveCustomField(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CustomField customField) {
			return ValidationHelper.Validate(customField);
		}
	}
}