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

		private IFundExpenseTypeService _FundExpenseTypeService;
		public IFundExpenseTypeService FundExpenseTypeService {
			get {
				if (_FundExpenseTypeService == null) {
					_FundExpenseTypeService = new FundExpenseTypeService();
				}
				return _FundExpenseTypeService;
			}
			set {
				_FundExpenseTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			FundExpenseTypeService.SaveFundExpenseType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(FundExpenseType fundExpenseType) {
			return ValidationHelper.Validate(fundExpenseType);
		}
	}
}