using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(FixedIncomeMD))]
	public partial class FixedIncome {
		public class FixedIncomeMD {
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

			[Required(ErrorMessage = "FixedIncomeTypeID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "FixedIncomeTypeID is required")]
			public global::System.Int32 FixedIncomeTypeID {
				get;
				set;
			}

			#endregion
		}

		public FixedIncome(IFixedIncomeService equityService)
			: this() {
			this.FixedIncomeService = equityService;
		}

		public FixedIncome() {
		}

		private IFixedIncomeService _equityService;
		public IFixedIncomeService FixedIncomeService {
			get {
				if (_equityService == null) {
					_equityService = new FixedIncomeService();
				}
				return _equityService;
			}
			set {
				_equityService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var equity = this;
			IEnumerable<ErrorInfo> errors = Validate(equity);
			if (errors.Any()) {
				return errors;
			}
			FixedIncomeService.SaveFixedIncome(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(FixedIncome equity) {
			return ValidationHelper.Validate(equity);
		}
	}
}