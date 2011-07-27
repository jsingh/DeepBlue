using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
 

namespace DeepBlue.Models.Entity {

	[MetadataType(typeof(UnderlyingDirectDocumentMD))]
	public partial class UnderlyingDirectDocument {
		public class UnderlyingDirectDocumentMD : CreatedByFields {
			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}
 

			[Required(ErrorMessage = "SecurityID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "SecurityID is required")]
			public global::System.Int32 SecurityID {
				get;
				set;
			}

			[Required(ErrorMessage = "SecurityTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "SecurityTypeID is required")]
			public global::System.Int32 SecurityTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "FileID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FileID is required")]
			public global::System.Int32 FileID {
				get;
				set;
			}

			[Required(ErrorMessage = "DocumentTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "DocumentTypeID is required")]
			public global::System.Int32 DocumentTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "DocumentDate is required")]
			[DateRange(ErrorMessage = "DocumentDate is required")]
			public global::System.DateTime DocumentDate {
				get;
				set;
			}

			#endregion
		}

		public UnderlyingDirectDocument(IUnderlyingDirectDocumentService underlyingDirectDocumentservice)
			: this() {
			this.underlyingDirectDocumentservice = underlyingDirectDocumentservice;
		}

		public UnderlyingDirectDocument() {
		}

		private IUnderlyingDirectDocumentService _underlyingDirectDocumentService;
		public IUnderlyingDirectDocumentService underlyingDirectDocumentservice {
			get {
				if (_underlyingDirectDocumentService == null) {
					_underlyingDirectDocumentService = new UnderlyingDirectDocumentService();
				}
				return _underlyingDirectDocumentService;
			}
			set {
				_underlyingDirectDocumentService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var underlyingDirectDocument = this;
			IEnumerable<ErrorInfo> errors = Validate(underlyingDirectDocument);
			if (errors.Any()) {
				return errors;
			}
			underlyingDirectDocumentservice.SaveUnderlyingDirectDocument(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingDirectDocument underlyingDirectDocument) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(underlyingDirectDocument);
			return errors;
		}
	}
}