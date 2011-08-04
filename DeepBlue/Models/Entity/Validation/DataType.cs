using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(DataTypeMD))]
	public partial class DataType {
		public class DataTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "DataType Name is required")]
			[StringLength(50, ErrorMessage = "DataType Name must be under 50 characters.")]
			public global::System.String DataTypeName {
				get;
				set;
			}
			#endregion
		}

		public DataType(IDataTypeService dataTypeService)
			: this() {
				this.DataTypeservice = dataTypeService;
		}

		public DataType() {
		}

		private IDataTypeService _DataTypeService;
		public IDataTypeService DataTypeservice {
			get {
				if (_DataTypeService == null) {
					_DataTypeService = new DataTypeService();
				}
				return _DataTypeService;
			}
			set {
				_DataTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			DataTypeservice.SaveDataType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DataType dataType) {
			return ValidationHelper.Validate(dataType);
		}
	}
}