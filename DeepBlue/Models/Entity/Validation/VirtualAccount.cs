using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue;


namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(VirtualAccountMD))]
	public partial class VirtualAccount {
		public class VirtualAccountMD {

			#region Primitive Properties

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}


			[Required(ErrorMessage = "FundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FundID is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}


			[Required(ErrorMessage = "AccountName is required")]
			[StringLength(50, ErrorMessage = "AccountName must be under 50 characters.")]
			public global::System.String AccountName {
				get;
				set;
			}


			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "ParentVirtualAccountID is required")]
			public Nullable<global::System.Int32> ParentVirtualAccountID {
				get;
				set;
			}


			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "ActualAccountID is required")]
			public Nullable<global::System.Int32> ActualAccountID {
				get;
				set;
			}


			[Required(ErrorMessage = "LedgerBalance is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "LedgerBalance is required")]
			public global::System.Decimal LedgerBalance {
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
		public VirtualAccount(IVirtualAccountService virtualAccountService)
			: this() {
			this.VirtualAccountService = virtualAccountService;
		}

		public VirtualAccount() {
		}

		private IVirtualAccountService _VirtualAccountService;
		public IVirtualAccountService VirtualAccountService {
			get {
				if (_VirtualAccountService == null) {
					_VirtualAccountService = new VirtualAccountService();
				}
				return _VirtualAccountService;
			}
			set {
				_VirtualAccountService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			VirtualAccountService.SaveVirtualAccount(this);
			return errors;
		}

		private IEnumerable<ErrorInfo> Validate(VirtualAccount virtualAccount) {
			return ValidationHelper.Validate(virtualAccount);
		}
	}

}