using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CashDistributionTypeMD))]
	public partial class CashDistributionType {
		public class CashDistributionTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "Name is required")]
			[StringLength(50, ErrorMessage = "Name must be under 50 characters.")]
			public global::System.String Name {
				get;
				set;
			}
			#endregion
		}

		public CashDistributionType(ICashDistributionTypeService cashDistributionTypeService)
			: this() {
			this.CashDistributionTypeService = cashDistributionTypeService;
		}

		public CashDistributionType() {
		}

		private ICashDistributionTypeService _CashDistributionTypeService;
		public ICashDistributionTypeService CashDistributionTypeService {
			get {
				if (_CashDistributionTypeService == null) {
					_CashDistributionTypeService = new CashDistributionTypeService();
				}
				return _CashDistributionTypeService;
			}
			set {
				_CashDistributionTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			CashDistributionTypeService.SaveCashDistributionType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CashDistributionType cashDistributionType) {
			return ValidationHelper.Validate(cashDistributionType);
		}
	}
}