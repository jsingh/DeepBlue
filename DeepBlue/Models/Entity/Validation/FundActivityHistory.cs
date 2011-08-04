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

		public FundActivityHistory(IFundActivityHistoryService fundHistoryService)
			: this() {
				this.FundActivityHistoryService = fundHistoryService;
		}

		public FundActivityHistory() {
		}

		private IFundActivityHistoryService _FundActivityHistoryService;
		public IFundActivityHistoryService FundActivityHistoryService {
			get {
				if (_FundActivityHistoryService == null) {
					_FundActivityHistoryService = new FundActivityHistoryService();
				}
				return _FundActivityHistoryService;
			}
			set {
				_FundActivityHistoryService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			FundActivityHistoryService.SaveFundActivityHistory(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(FundActivityHistory fundActivityHistory) {
			return ValidationHelper.Validate(fundActivityHistory);
		}
	}
}