using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeepBlue.Models.Entity;

namespace DeepBlue.Models.Investor {
	public class CustomFieldModel {

		public int Key { get; set; }

		public List<CustomField> Fields { get; set; }

		public List<CustomFieldValueDetail> Values { get; set; }

	}

	public class CustomFieldValueDetail {

		public int CustomFieldId { get; set; }

		public int DataTypeId { get; set; }
		
		public int CustomFieldValueId { get; set; }

		public int? IntegerValue { get; set; }

		public string TextValue { get; set; }

		public bool? BooleanValue { get; set; }

		public decimal? CurrencyValue { get; set; }

		public string DateValue { get; set; }
	}
}