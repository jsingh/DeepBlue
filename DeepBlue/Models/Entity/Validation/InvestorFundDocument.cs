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

		public InvestorFundDocument(IInvestorFundDocumentService investorService)
			: this() {
			this.InvestorFundDocumentService = investorService;
		}

		public InvestorFundDocument() {

		}

		private IInvestorFundDocumentService _investorFundDocumentDocumentService;
		public IInvestorFundDocumentService InvestorFundDocumentService {
			get {
				if (_investorFundDocumentDocumentService == null) {
					_investorFundDocumentDocumentService = new InvestorFundDocumentService();
				}
				return _investorFundDocumentDocumentService;
			}
			set {
				_investorFundDocumentDocumentService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var investorFundDocument = this;
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(investorFundDocument);
			errors = errors.Union(ValidationHelper.Validate(investorFundDocument.File));
			if (errors.Any()) {
				return errors;
			}
			InvestorFundDocumentService.SaveInvestorFundDocument(this);
			return null;
		}

	}

}