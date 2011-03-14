using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Admin {
	public class EditCustomFieldModel {

		public int CustomFieldId { get; set; }

		public int ModuleId { get; set; }

		public int DataTypeId { get; set; }

		public string CustomFieldText { get; set; }

		public string OptionalText { get; set; }

		public bool Search { get; set; }
	}
}