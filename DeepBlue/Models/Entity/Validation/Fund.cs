using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue;


namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(FundMD))]
	public partial class Fund {
		public class FundMD {
			#region Primitive Properties

			[Required]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue)]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[StringLength(50), Required]
			public global::System.String FundName {
				get;
				set;
			}

			[StringLength(50), Required]
			public global::System.String TaxId {
				get;
				set;
			}
			
			[Required]
			public global::System.DateTime InceptionDate {
				get;
				set;
			}

			#endregion
		}

		public Fund(IFundService fundService)
			: this() {
			this.FundService = fundService;
		}

		public Fund() {
		}

		private IFundService _fundService;
		public IFundService FundService {
			get {
				if (_fundService == null) {
					_fundService = new FundService();
				}
				return _fundService;
			}
			set {
				_fundService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var fund = this;
			IEnumerable<ErrorInfo> errors = Validate(fund);
			if (errors.Any()) {
				return errors;
			}
			FundService.SaveFund(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(Fund fund) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(fund);
			foreach (FundAccount account in fund.FundAccounts) {
				errors.Union(ValidationHelper.Validate(account));
			}
			return errors;
		}
	}

	[MetadataType(typeof(FundAccountMD))]
	public partial class FundAccount {
		public class FundAccountMD {
			#region Primitive Properties
			[Required]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue)]
			public global::System.Int32 EntityID {
				get;
				set;
			}
			
			[StringLength(50), Required]
			public global::System.String BankName {
				get;
				set;
			}

			[StringLength(20), Required]
			public global::System.String Account {
				get;
				set;
			}
			#endregion
		}
	}
	 
}