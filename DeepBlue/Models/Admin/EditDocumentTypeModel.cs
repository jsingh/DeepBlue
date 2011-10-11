using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DeepBlue.Helpers;
using System.Web.Mvc;


namespace DeepBlue.Models.Admin {
	public class EditDocumentTypeModel {
		public EditDocumentTypeModel() {
			DocumentTypeID = 0;
			DocumentTypeName = string.Empty;
			DocumentSectionID = 0;
		}

		public int DocumentTypeID { get; set; }

		[Required(ErrorMessage = "Document Section is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Document Section is required")]
		[DisplayName("Document Section")]
		public int? DocumentSectionID { get; set; }

		public List<SelectListItem> DocumentSections { get; set; }

		[Required(ErrorMessage = "Document Type is required")]
		[StringLength(20, ErrorMessage = "Document Type must be under 20 characters.")]
		[DisplayName("Document Type Name")]
		public string DocumentTypeName { get; set; }

	}
}