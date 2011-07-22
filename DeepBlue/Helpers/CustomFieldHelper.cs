using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;
using DeepBlue.Controllers.Admin;

namespace DeepBlue.Helpers {

	public class CustomFieldModel {
		public CustomFieldModel() {
			InitializeDatePicker = true;
			DisplayTwoColumn = false;
		}
		public int Key { get; set; }

		public List<CustomFieldDetail> Fields { get; set; }

		public List<CustomFieldValueDetail> Values { get; set; }

		public bool InitializeDatePicker { get; set; }

		public bool DisplayTwoColumn { get; set; }
	}

	public class CustomFieldDetail {

		public int CustomFieldId { get; set; }

		public int ModuleId { get; set; }

		public int DataTypeId { get; set; }

		public bool Search { get; set; }

		public string CustomFieldText { get; set; }

		public string OptionalText { get; set; }
	}

	public class CustomFieldValueDetail {

		public CustomFieldValueDetail() {
			CustomFieldId = 0;
			Key = 0;
			DataTypeId = 0;
			CustomFieldValueId = 0;
			IntegerValue = 0;
			TextValue = string.Empty;
			BooleanValue = false;
			CurrencyValue = 0;
			DateValue = string.Empty;
		}

		public int CustomFieldId { get; set; }

		public int Key { get; set; }

		public int DataTypeId { get; set; }

		public int CustomFieldValueId { get; set; }

		public int IntegerValue { get; set; }

		public string TextValue { get; set; }

		public bool BooleanValue { get; set; }

		public decimal CurrencyValue { get; set; }

		public string DateValue { get; set; }
	}
}