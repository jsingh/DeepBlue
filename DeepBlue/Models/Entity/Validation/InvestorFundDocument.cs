using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue;


namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(InvestorFundDocumentMD))]
	public partial class InvestorFundDocument {
		public class InvestorFundDocumentMD : CreatedByFields {

			#region Primitive Properties
			[Required(ErrorMessage = "Document Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Document Type is required")]
			public global::System.Int32 DocumentTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "Document Date is required")]
			[DateRange(ErrorMessage = "Document Date is required")]
			public global::System.DateTime DocumentDate {
				get;
				set;
			}

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			#endregion
		}

		public InvestorFundDocument(IInvestorFundDocumentService investorFundDocumentService)
			: this() {
			this.InvestorFundDocumentService = investorFundDocumentService;
		}

		public InvestorFundDocument() {
		}

		private IInvestorFundDocumentService _InvestorFundDocumentDocumentService;
		public IInvestorFundDocumentService InvestorFundDocumentService {
			get {
				if (_InvestorFundDocumentDocumentService == null) {
					_InvestorFundDocumentDocumentService = new InvestorFundDocumentService();
				}
				return _InvestorFundDocumentDocumentService;
			}
			set {
				_InvestorFundDocumentDocumentService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			InvestorFundDocumentService.SaveInvestorFundDocument(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(InvestorFundDocument investorFundDocument) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(investorFundDocument);
			if (investorFundDocument.File != null) {
				errors = errors.Union(ValidationHelper.Validate(investorFundDocument.File));
			}
			return errors;
		}
	}

}