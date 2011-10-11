using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {

	[MetadataType(typeof(DocumentTypeMD))]
	public partial class DocumentType {
		public class DocumentTypeMD {

			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "DocumentTypeName is required")]
			[StringLength(50, ErrorMessage = "DocumentTypeName must be under 50 characters.")]
			public global::System.String DocumentTypeName {
				get;
				set;
			}

			[StringLength(100, ErrorMessage = "Description must be under 100 characters.")]
			public global::System.String Description {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "DocumentSectionID is required")]
			public Nullable<global::System.Int32> DocumentSectionID {
				get;
				set;
			}

			#endregion
		}
		
		public DocumentType(IDocumentTypeService documentTypeService)
			: this() {
			this.DocumentTypeService = documentTypeService;
		}

		public DocumentType() {
		}

		private IDocumentTypeService _DocumentTypeService;
		public IDocumentTypeService DocumentTypeService {
			get {
				if (_DocumentTypeService == null) {
					_DocumentTypeService = new DocumentTypeService();
				}
				return _DocumentTypeService;
			}
			set {
				_DocumentTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			DocumentTypeService.SaveDocumentType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DocumentType documentType) {
			return ValidationHelper.Validate(documentType);
		}
	}
}

