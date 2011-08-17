using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Admin {
	public class EditCustomFieldModel {

		public int CustomFieldId { get; set; }

		[Required(ErrorMessage = "Custom Field is required.")]
		[StringLength(50, ErrorMessage = "Custom Field must be under 50 characters.")]
		[DisplayName("Name:")]
		public string CustomFieldText { get; set; }

		[Required(ErrorMessage = "Module is required.")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Module is required")]
		[DisplayName("Module:")]
		public int ModuleId { get; set; }

		[Required(ErrorMessage = "Data Type is required.")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Data Type is required")]
		[DisplayName("DataType:")]
		public int DataTypeId { get; set; }

		[StringLength(2000, ErrorMessage = "Optional Text must be under 2000 characters.")]
		[DisplayName("Optional:")]
		public string OptionalText { get; set; }

		[DisplayName("Search:")]
		public bool Search { get; set; }

		public List<SelectListItem> Modules { get; set; }

		public List<SelectListItem> DataTypes { get; set; }

		public List<EditOptionFieldModel> OptionFields { get; set; }
	}

	public class EditOptionFieldModel {

		public EditOptionFieldModel(){
			Index++;
		}

		public int Index { get; private set; }
		
		public int OptionFieldId { get; set; }

		public int CustomFieldId { get; set; }

		[DisplayName("Text:")]
		public string OptionText { get; set; }

		[DisplayName("Default:")]
		public bool IsDefault { get; set; }

		public int SortOrder { get; set; }
	}
}