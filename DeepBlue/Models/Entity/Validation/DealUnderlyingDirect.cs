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

			[Required(ErrorMessage = "SecurityTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "SecurityTypeID is required")]
			public global::System.Int32 SecurityTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "SecurityID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "SecurityID is required")]
			public global::System.Int32 SecurityID {
				get;
				set;
			}

			[DateRange()]
			public global::System.DateTime RecordDate {
				get;
				set;
			}

			[Range((int)1, int.MaxValue, ErrorMessage = "NumberOfShares is required")]
			public global::System.Int32 NumberOfShares {
				get;
				set;
			}
			#endregion
		}

		public DealUnderlyingDirect(IDealUnderlyingDirectService dealUnderlyingDirectService)
			: this() {
			this.DealUnderlyingDirectService = dealUnderlyingDirectService;
		}

		public DealUnderlyingDirect() {
		}

		private IDealUnderlyingDirectService _DealUnderlyingDirectService;
		public IDealUnderlyingDirectService DealUnderlyingDirectService {
			get {
				if (_DealUnderlyingDirectService == null) {
					_DealUnderlyingDirectService = new DealUnderlyingDirectService();
				}
				return _DealUnderlyingDirectService;
			}
			set {
				_DealUnderlyingDirectService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			DealUnderlyingDirectService.SaveDealUnderlyingDirect(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(DealUnderlyingDirect dealUnderlyingDirect) {
			return ValidationHelper.Validate(dealUnderlyingDirect);
		}
	}
}