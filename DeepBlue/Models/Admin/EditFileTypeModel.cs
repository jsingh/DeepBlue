using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;


namespace DeepBlue.Models.Admin {
	public class EditFileTypeModel {
		public EditFileTypeModel() {
			FileTypeId = 0;
			FileTypeName = string.Empty;
			FileExtension = string.Empty;
		}

		public int FileTypeId { get; set; }

		[Required(ErrorMessage = "Name is required")]
		[StringLength(50, ErrorMessage = "Name must be under 50 characters.")]
		[RemoteUID_(Action = "FileTypeNameAvailable", Controller = "Admin", ValidateParameterName = "FileTypeName", Params = new string[] { "FileTypeId" })]
		[DisplayName("Name:")]
		public string FileTypeName { get; set; }

		[StringLength(20, ErrorMessage = "File Extension must be under 20 characters.")]
		[DisplayName("File Extension:")]
		public string FileExtension { get; set; }

		[StringLength(100, ErrorMessage = "Description must be under 100 characters.")]
		[DisplayName("Description:")]
		public string Description { get; set; }

	}
}