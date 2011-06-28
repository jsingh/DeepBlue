using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(FundExpenseTypeMD))]
	public partial class FundExpenseType {
		public class FundExpenseTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "Name is required")]
			[StringLength(50, ErrorMessage = "Name must be under 50 characters.")]
			public global::System.String Name {
				get;
				set;
			}

			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}
			#endregion
		}

		public FundExpenseType(IFundExpenseTypeService fundExpenseTypeService)
			: this() {
			this.FundExpenseTypeService = fundExpenseTypeService;
		}

		public FundExpenseType() {
		}

		private IFundExpenseTypeService _fundExpenseTypeService;
		public IFundExpenseTypeService FundExpenseTypeService {
			get {
				if (_fundExpenseTypeService == null) {
					_fundExpenseTypeService = new FundExpenseTypeService();
				}
				return _fundExpenseTypeService;
			}
			set {
				_fundExpenseTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var fundExpenseType = this;
			IEnumerable<ErrorInfo> errors = Validate(fundExpenseType);
			if (errors.Any()) {
				return errors;
			}
			FundExpenseTypeService.SaveFundExpenseType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(FundExpenseType fundExpenseType) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(fundExpenseType);
			return errors;
		}
	}
}