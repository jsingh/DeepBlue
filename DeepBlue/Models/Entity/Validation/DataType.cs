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
			[StringLength(50), Required]
			public global::System.String DataTypeName {
				get;
				set;
			}
			#endregion
		}

		public DataType(IDataTypeService dataTypeservice)
			: this() {
			this.dataTypeservice = dataTypeservice;
		}

		public DataType() {
		}

		private IDataTypeService _dataTypeService;
		public IDataTypeService dataTypeservice {
			get {
				if (_dataTypeService == null) {
					_dataTypeService = new DataTypeService();
				}
				return _dataTypeService;
			}
			set {
				_dataTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var dataType = this;
			IEnumerable<ErrorInfo> errors = Validate(dataType);
			if (errors.Any()) {
				return errors;
			}
			dataTypeservice.SaveDataType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DataType dataTypeclosing) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(dataTypeclosing);
			return errors;
		}
	}
}