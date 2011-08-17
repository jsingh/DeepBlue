using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using System.ComponentModel;

namespace DeepBlue.Models.Admin {
	public class EditDataTypeModel {
		
		public int DataTypeId { get; set; }

		[Required(ErrorMessage = "Data Type is required.")]
		[StringLength(50, ErrorMessage = "Data Type must be under 50 characters.")]
		[DisplayName("Name:")]
		public string DataTypeName { get; set; }
	}
}