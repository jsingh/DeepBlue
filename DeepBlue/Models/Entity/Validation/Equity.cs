using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(EquityMD))]
	public partial class Equity {
		public class EquityMD {
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

			[Required(ErrorMessage = "EquityTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "EquityTypeID is required")]
			public global::System.Int32 EquityTypeID {
				get;
				set;
			}

			[Required(ErrorMessage = "Symbol is required")]
			[StringLength(50, ErrorMessage = "Symbol must be under 50 characters.")]
			public global::System.String Symbol {
				get;
				set;
			}
			#endregion
		}

		public Equity(IEquityService equityService)
			: this() {
			this.EquityService = equityService;
		}

		public Equity() {
		}

		private IEquityService _EquityService;
		public IEquityService EquityService {
			get {
				if (_EquityService == null) {
					_EquityService = new EquityService();
				}
				return _EquityService;
			}
			set {
				_EquityService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			EquityService.SaveEquity(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(Equity equity) {
			return ValidationHelper.Validate(equity);
		}
	}
}