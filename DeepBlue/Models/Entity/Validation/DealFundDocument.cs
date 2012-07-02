using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
 
namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(DealFundDocumentMD))]
	public partial class DealFundDocument {
		public class DealFundDocumentMD : CreatedByFields {
			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "DealID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "DealID is required")]
			public global::System.Int32 DealID {
				get;
				set;
			}

			//[Required(ErrorMessage = "FileID is required")]
			//[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FileID is required")]
			//public global::System.Int32 FileID {
			//    get;
			//    set;
			//}

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

		public DealFundDocument(IDealFundDocumentService dealFundDocumentService)
			: this() {
				this.DealFundDocumentservice = dealFundDocumentService;
		}

		public DealFundDocument() {
		}

		private IDealFundDocumentService _DealFundDocumentService;
		public IDealFundDocumentService DealFundDocumentservice {
			get {
				if (_DealFundDocumentService == null) {
					_DealFundDocumentService = new DealFundDocumentService();
				}
				return _DealFundDocumentService;
			}
			set {
				_DealFundDocumentService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			DealFundDocumentservice.SaveDealFundDocument(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DealFundDocument dealFundDocument) {
			return ValidationHelper.Validate(dealFundDocument);
		}
	}
}