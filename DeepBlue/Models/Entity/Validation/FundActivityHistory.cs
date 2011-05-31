using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(FundActivityHistoryMD))]
	public partial class FundActivityHistory {
		public class FundActivityHistoryMD {
			#region Primitive Properties
			[Required(ErrorMessage = "Fund is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required(ErrorMessage = "OldNumberOfShares is required")]
			[Range((int)0, int.MaxValue, ErrorMessage = "OldNumberOfShares is required")]
			public global::System.Int32 OldNumberOfShares {
				get;
				set;
			}

			[Required(ErrorMessage = "NewNumberOfShares is required")]
			[Range((int)0, int.MaxValue, ErrorMessage = "NewNumberOfShares is required")]
			public global::System.Int32 NewNumberOfShares {
				get;
				set;
			}

			[Required(ErrorMessage = "ActivityID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "ActivityID is required")]
			public global::System.Int32 ActivityID {
				get;
				set;
			}

			[Required(ErrorMessage = "ActivityTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "ActivityTypeID is required")]
			public global::System.Int32 ActivityTypeID {
				get;
				set;
			}
			#endregion
		}

		public FundActivityHistory(IFundActivityHistoryService fundservice)
			: this() {
			this.fundActivityHistoryservice = fundservice;
		}

		public FundActivityHistory() {
		}

		private IFundActivityHistoryService _fundActivityHistoryService;
		public IFundActivityHistoryService fundActivityHistoryservice {
			get {
				if (_fundActivityHistoryService == null) {
					_fundActivityHistoryService = new FundActivityHistoryService();
				}
				return _fundActivityHistoryService;
			}
			set {
				_fundActivityHistoryService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var fundActivityHistory = this;
			IEnumerable<ErrorInfo> errors = Validate(fundActivityHistory);
			if (errors.Any()) {
				return errors;
			}
			fundActivityHistoryservice.SaveFundActivityHistory(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(FundActivityHistory fundActivityHistory) {
			return ValidationHelper.Validate(fundActivityHistory);
		}
	}
}