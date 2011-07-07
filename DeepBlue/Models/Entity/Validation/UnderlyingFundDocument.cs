using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
 

namespace DeepBlue.Models.Entity {

	[MetadataType(typeof(UnderlyingFundDocumentMD))]
	public partial class UnderlyingFundDocument {
		public class UnderlyingFundDocumentMD : CreatedByFields {
			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "UnderlyingFundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "UnderlyingFundID is required")]
			public global::System.Int32 UnderlyingFundID {
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

		public UnderlyingFundDocument(IUnderlyingFundDocumentService underlyingFundDocumentservice)
			: this() {
			this.underlyingFundDocumentservice = underlyingFundDocumentservice;
		}

		public UnderlyingFundDocument() {
		}

		private IUnderlyingFundDocumentService _underlyingFundDocumentService;
		public IUnderlyingFundDocumentService underlyingFundDocumentservice {
			get {
				if (_underlyingFundDocumentService == null) {
					_underlyingFundDocumentService = new UnderlyingFundDocumentService();
				}
				return _underlyingFundDocumentService;
			}
			set {
				_underlyingFundDocumentService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var underlyingFundDocument = this;
			IEnumerable<ErrorInfo> errors = Validate(underlyingFundDocument);
			if (errors.Any()) {
				return errors;
			}
			underlyingFundDocumentservice.SaveUnderlyingFundDocument(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFundDocument underlyingFundDocument) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(underlyingFundDocument);
			return errors;
		}
	}
}