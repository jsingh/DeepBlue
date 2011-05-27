using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(DealUnderlyingDirectMD))]
	public partial class DealUnderlyingDirect {
		public class DealUnderlyingDirectMD {
			#region Primitive Properties
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

			[Range(typeof(decimal),"0", "79228162514264337593543950335", ErrorMessage = "FMV is required")]
			public global::System.Decimal FMV {
				get;
				set;
			}

			[Range(typeof(decimal),"0", "100", ErrorMessage = "Percent is required")]
			public global::System.Decimal Percent {
				get;
				set;
			}

			[Range((int)0, int.MaxValue, ErrorMessage = "NumberOfShares is required")]
			public global::System.Int32 NumberOfShares {
				get;
				set;
			}
			#endregion
		}

		public DealUnderlyingDirect(IDealUnderlyingDirectService purchaseTypeService)
			: this() {
			this.DealUnderlyingDirectService = purchaseTypeService;
		}

		public DealUnderlyingDirect() {
		}

		private IDealUnderlyingDirectService _purchaseTypeService;
		public IDealUnderlyingDirectService DealUnderlyingDirectService {
			get {
				if (_purchaseTypeService == null) {
					_purchaseTypeService = new DealUnderlyingDirectService();
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
			DealUnderlyingDirectService.SaveDealUnderlyingDirect(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DealUnderlyingDirect purchaseType) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(purchaseType);
			return errors;
		}
	}
}