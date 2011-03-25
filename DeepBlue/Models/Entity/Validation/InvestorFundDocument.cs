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
		public class InvestorFundDocumentMD {

			#region Primitive Properties
			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 DocumentTypeID {
				get;
				set;
			}

			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 InvestorID {
				get;
				set;
			}

			[Required]
			[Range(typeof(DateTime), "1/1/1900", "1/1/9999")]
			public global::System.DateTime DocumentDate {
				get;
				set;
			}

			[Required]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue)]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 CreatedBy {
				get;
				set;
			}

			[Required]
			[Range(typeof(DateTime), "1/1/1900", "1/1/9999")]
			public global::System.DateTime CreatedDate {
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
			errors.Union(ValidationHelper.Validate(investorFundDocument.File));
			if (errors.Any()) {
				return errors;
			}
			InvestorFundDocumentService.SaveInvestorFundDocument(this);
			return null;
		}

	}

	[MetadataType(typeof(FileMD))]
	public partial class File {
		public class FileMD {
			#region Primitive Properties
			[Required]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue)]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[StringLength(500)]
			public global::System.String FilePath {
				get;
				set;
			}

			[StringLength(200)]
			public global::System.String FileName {
				get;
				set;
			}

			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 FileTypeId {
				get;
				set;
			}

			[Required]
			[Range(typeof(DateTime), "1/1/1900", "1/1/9999")]
			public global::System.DateTime CreatedDate {
				get;
				set;
			}

			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 CreatedBy {
				get;
				set;
			}
			#endregion
		}
	}

}