using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(DealUnderlyingFundMD))]
	public partial class DealUnderlyingFund {
		public class DealUnderlyingFundMD {
			#region Primitive Properties
			[Required(ErrorMessage = "UnderlyingFundID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "UnderlyingFundID is required")]
			public global::System.Int32 UnderlyingFundID {
				get;
				set;
			}

			[Required(ErrorMessage = "Deal is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Deal is required")]
			public global::System.Int32 DealID {
				get;
				set;
			}

			[Required(ErrorMessage = "RecordDate is required")]
			[DateRange()]
			public global::System.DateTime RecordDate {
				get;
				set;
			}

            [Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "FundNav is required")]
			public global::System.Decimal FundNav {
				get;
				set;
			}

            [Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "Percent is required")]
			public global::System.Decimal Percent {
				get;
				set;
			}

            [Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "CommittedAmount is required")]
			public global::System.Decimal CommittedAmount {
				get;
				set;
			}
			#endregion
		}

		public DealUnderlyingFund(IDealUnderlyingFundService purchaseTypeService)
			: this() {
			this.DealUnderlyingFundService = purchaseTypeService;
		}

		public DealUnderlyingFund() {
		}

		private IDealUnderlyingFundService _purchaseTypeService;
		public IDealUnderlyingFundService DealUnderlyingFundService {
			get {
				if (_purchaseTypeService == null) {
					_purchaseTypeService = new DealUnderlyingFundService();
				}
				return _purchaseTypeService;
			}
			set {
				_purchaseTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var purchaseType = this;
			IEnumerable<ErrorInfo> errors = Validate(purchaseType);
			if (errors.Any()) {
				return errors;
			}
			DealUnderlyingFundService.SaveDealUnderlyingFund(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DealUnderlyingFund purchaseType) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(purchaseType);
			return errors;
		}
	}
}