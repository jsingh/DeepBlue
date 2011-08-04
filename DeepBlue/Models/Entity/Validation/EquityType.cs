using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(EquityTypeMD))]
	public partial class EquityType {
		public class EquityTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Equity is required")]
			[StringLength(100, ErrorMessage = "Equity must be under 100 characters.")]
			public global::System.String Equity {
				get;
				set;
			}
			#endregion
		}

		public EquityType(IEquityTypeService equityTypeService)
			: this() {
			this.EquityTypeService = equityTypeService;
		}

		public EquityType() {
		}

		private IEquityTypeService _EquityTypeService;
		public IEquityTypeService EquityTypeService {
			get {
				if (_EquityTypeService == null) {
					_EquityTypeService = new EquityTypeService();
				}
				return _EquityTypeService;
			}
			set {
				_EquityTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			EquityTypeService.SaveEquityType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(EquityType equityType) {
			return ValidationHelper.Validate(equityType);
		}
	}
}