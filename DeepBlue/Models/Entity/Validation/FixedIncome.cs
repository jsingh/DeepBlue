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

		public FixedIncome(IFixedIncomeService fixedIncomeService)
			: this() {
			this.FixedIncomeService = fixedIncomeService;
		}

		public FixedIncome() {
		}

		private IFixedIncomeService _FixedIncomeService;
		public IFixedIncomeService FixedIncomeService {
			get {
				if (_FixedIncomeService == null) {
					_FixedIncomeService = new FixedIncomeService();
				}
				return _FixedIncomeService;
			}
			set {
				_FixedIncomeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			FixedIncomeService.SaveFixedIncome(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(FixedIncome fixedIncome) {
			return ValidationHelper.Validate(fixedIncome);
		}
	}
}