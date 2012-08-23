using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(AccountingEntryTemplateMD))]
	public partial class AccountingEntryTemplate {
		public class AccountingEntryTemplateMD {

			#region Primitive Properties

				[Required(ErrorMessage = "FundID is required")]
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


			[Required(ErrorMessage = "AccountingTransactionTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "AccountingTransactionTypeID is required")]
			public global::System.Int32 AccountingTransactionTypeID {
				get;
				set;
			}


			[Required(ErrorMessage = "VirtualAccountID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "VirtualAccountID is required")]
			public global::System.Int32 VirtualAccountID {
				get;
				set;
			}


			[StringLength(100, ErrorMessage = "Description must be under 100 characters.")]
			public global::System.String Description {
				get;
				set;
			}


			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Amount is required")]
			public Nullable<global::System.Decimal> Amount {
				get;
				set;
			}


			[Required(ErrorMessage = "AccountingEntryAmountTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "AccountingEntryAmountTypeID is required")]
			public global::System.Int32 AccountingEntryAmountTypeID {
				get;
				set;
			}


			[StringLength(50, ErrorMessage = "AccountingEntryAmountTypeData must be under 50 characters.")]
			public global::System.String AccountingEntryAmountTypeData {
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
		public AccountingEntryTemplate(IAccountingEntryTemplateService accountingEntryTemplateService)
			: this() {
			this.AccountingEntryTemplateService = accountingEntryTemplateService;
		}

		public AccountingEntryTemplate() {
		}

		private IAccountingEntryTemplateService _AccountingEntryTemplateService;
		public IAccountingEntryTemplateService AccountingEntryTemplateService {
			get {
				if (_AccountingEntryTemplateService == null) {
					_AccountingEntryTemplateService = new AccountingEntryTemplateService();
				}
				return _AccountingEntryTemplateService;
			}
			set {
				_AccountingEntryTemplateService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			AccountingEntryTemplateService.SaveAccountingEntryTemplate(this);
			return errors;
		}

		private IEnumerable<ErrorInfo> Validate(AccountingEntryTemplate accountingEntryTemplate) {
			return ValidationHelper.Validate(accountingEntryTemplate);
		}
	}
}
