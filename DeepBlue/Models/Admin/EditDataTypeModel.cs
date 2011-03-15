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

		[Required(ErrorMessage = "Name is required.")]
		[StringLength(50, ErrorMessage = "Name Must is under 50 characters.")]
		[RemoteUID_(Action = "DataTypeNameAvailable", Controller = "Admin", ValidateParameterName = "DataTypeName", Params = new string[] { "DataTypeId" })]
		[DisplayName("Name:")]
		public string DataTypeName { get; set; }
	}
}