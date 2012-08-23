using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(AccountingEntryMD))]
	public partial class AccountingEntry {
		public class AccountingEntryMD {

			#region Primitive Properties

			[Required(ErrorMessage = "AccountingEntryTemplateID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "AccountingEntryTemplateID is required")]
			public global::System.Int32 AccountingEntryTemplateID {
				get;
				set;
			}


			[Required(ErrorMessage = "Amount is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Amount is required")]
			public global::System.Decimal Amount {
				get;
				set;
			}


			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "TraceID is required")]
			public Nullable<global::System.Int32> TraceID {
				get;
				set;
			}


			[Range((int)0, int.MaxValue, ErrorMessage = "AttributedTo is required")]
			public Nullable<global::System.Int32> AttributedTo {
				get;
				set;
			}


			[StringLength(100, ErrorMessage = "AttributedToName must be under 100 characters.")]
			public global::System.String AttributedToName {
				get;
				set;
			}


			[StringLength(50, ErrorMessage = "AttributedToType must be under 50 characters.")]
			public global::System.String AttributedToType {
				get;
				set;
			}


			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FundID is required")]
			public Nullable<global::System.Int32> FundID {
				get;
				set;
			}


			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}


			[Required(ErrorMessage = "CreatedDate is required")]
			[DateRange(ErrorMessage = "CreatedDate is required")]
			public global::System.DateTime CreatedDate {
				get;
				set;
			}


			[Required(ErrorMessage = "CreatedBy is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CreatedBy is required")]
			public global::System.Int32 CreatedBy {
				get;
				set;
			}


			[DateRange(ErrorMessage = "LastUpdatedDate is required")]
			public Nullable<global::System.DateTime> LastUpdatedDate {
				get;
				set;
			}


			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "LastUpdatedBy is required")]
			public Nullable<global::System.Int32> LastUpdatedBy {
				get;
				set;
			}

			#endregion
		}
		public AccountingEntry(IAccountingEntryService accountingEntryService)
			: this() {
			this.AccountingEntryService = accountingEntryService;
		}

		public AccountingEntry() {
		}

		private IAccountingEntryService _AccountingEntryService;
		public IAccountingEntryService AccountingEntryService {
			get {
				if (_AccountingEntryService == null) {
					_AccountingEntryService = new AccountingEntryService();
				}
				return _AccountingEntryService;
			}
			set {
				_AccountingEntryService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			AccountingEntryService.SaveAccountingEntry(this);
			return errors;
		}

		private IEnumerable<ErrorInfo> Validate(AccountingEntry accountingEntry) {
			return ValidationHelper.Validate(accountingEntry);
		}
	}
}