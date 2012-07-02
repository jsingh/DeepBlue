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
				this.UnderlyingFundDocumentservice = underlyingFundDocumentservice;
		}

		public UnderlyingFundDocument() {
		}

		private IUnderlyingFundDocumentService _UnderlyingFundDocumentService;
		public IUnderlyingFundDocumentService UnderlyingFundDocumentservice {
			get {
				if (_UnderlyingFundDocumentService == null) {
					_UnderlyingFundDocumentService = new UnderlyingFundDocumentService();
				}
				return _UnderlyingFundDocumentService;
			}
			set {
				_UnderlyingFundDocumentService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingFundDocumentservice.SaveUnderlyingFundDocument(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFundDocument underlyingFundDocument) {
			return ValidationHelper.Validate(underlyingFundDocument);
		}
	}
}