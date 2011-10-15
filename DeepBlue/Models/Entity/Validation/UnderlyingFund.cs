using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity.Partial;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingFundMD))]
	public partial class UnderlyingFund {
		public class UnderlyingFundMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "IssuerID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "IssuerID is required")]
			public global::System.Int32 IssuerID {
				get;
				set;
			}

			[Required(ErrorMessage = "FundName is required")]
			[StringLength(100, ErrorMessage = "FundName must be under 100 characters.")]
			public global::System.String FundName {
				get;
				set;
			}

			[Required(ErrorMessage = "FundTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FundTypeID is required")]
			public global::System.Int32 FundTypeID {
				get;
				set;
			}

			[Range((int)0, int.MaxValue, ErrorMessage = "VintageYear is required")]
			public Nullable<global::System.Int32> VintageYear {
				get;
				set;
			}

			[Range((int)0, int.MaxValue, ErrorMessage = "TotalSize is required")]
			public Nullable<global::System.Int32> TotalSize {
				get;
				set;
			}

			[Range((int)0, int.MaxValue, ErrorMessage = "TerminationYear is required")]
			public Nullable<global::System.Int32> TerminationYear {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "IndustryID is required")]
			public Nullable<global::System.Int32> IndustryID {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "GeographyID is required")]
			public Nullable<global::System.Int32> GeographyID {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "ReportingFrequencyID is required")]
			public Nullable<global::System.Int32> ReportingFrequencyID {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "ReportingTypeID is required")]
			public Nullable<global::System.Int32> ReportingTypeID {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "AccountID is required")]
			public Nullable<global::System.Int32> AccountID {
				get;
				set;
			}

			[StringLength(100, ErrorMessage = "LegalFundName must be under 100 characters.")]
			public global::System.String LegalFundName {
				get;
				set;
			}

			[StringLength(100, ErrorMessage = "Description must be under 100 characters.")]
			public global::System.String Description {
				get;
				set;
			}

			[DateRange(ErrorMessage = "FiscalYearEnd is required")]
			public Nullable<global::System.DateTime> FiscalYearEnd {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FundStructureID is required")]
			public Nullable<global::System.Int32> FundStructureID {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FundRegisteredOfficeID is required")]
			public Nullable<global::System.Int32> FundRegisteredOfficeID {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "AddressID is required")]
			public Nullable<global::System.Int32> AddressID {
				get;
				set;
			}

			[Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "ManagementFee is required")]
			public Nullable<global::System.Decimal> ManagementFee {
				get;
				set;
			}

			[Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "IncentiveFee is required")]
			public Nullable<global::System.Decimal> IncentiveFee {
				get;
				set;
			}

			[Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "TaxRate is required")]
			public Nullable<global::System.Decimal> TaxRate {
				get;
				set;
			}

			[StringLength(75, ErrorMessage = "AuditorName must be under 75 characters.")]
			public global::System.String AuditorName {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "ManagerContactID is required")]
			public Nullable<global::System.Int32> ManagerContactID {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "ShareClassTypeID is required")]
			public Nullable<global::System.Int32> ShareClassTypeID {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "InvestmentTypeID is required")]
			public Nullable<global::System.Int32> InvestmentTypeID {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "WebUserName must be under 50 characters.")]
			public global::System.String WebUserName {
				get;
				set;
			}

			[StringLength(50, ErrorMessage = "WebPassword must be under 50 characters.")]
			public global::System.String WebPassword {
				get;
				set;
			}

			[StringLength(100, ErrorMessage = "Website must be under 100 characters.")]
			public global::System.String Website {
				get;
				set;
			}

			#endregion
		}

		public UnderlyingFund(IUnderlyingFundService underlyingFundService)
			: this() {
			this.UnderlyingFundService = underlyingFundService;
		}

		public UnderlyingFund() {
		}

		private IUnderlyingFundService _UnderlyingFundService;
		public IUnderlyingFundService UnderlyingFundService {
			get {
				if (_UnderlyingFundService == null) {
					_UnderlyingFundService = new UnderlyingFundService();
				}
				return _UnderlyingFundService;
			}
			set {
				_UnderlyingFundService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingFundService.SaveUnderlyingFund(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFund underlyingFund) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(underlyingFund);
			if (underlyingFund.Account != null) {
				errors = errors.Union(ValidationHelper.Validate(underlyingFund.Account));
			}
			return errors;
		}
	}
}