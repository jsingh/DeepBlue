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

		private ICashDistributionTypeService _cashDistributionTypeService;
		public ICashDistributionTypeService CashDistributionTypeService {
			get {
				if (_cashDistributionTypeService == null) {
					_cashDistributionTypeService = new CashDistributionTypeService();
				}
				return _cashDistributionTypeService;
			}
			set {
				_cashDistributionTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var cashDistributionType = this;
			IEnumerable<ErrorInfo> errors = Validate(cashDistributionType);
			if (errors.Any()) {
				return errors;
			}
			CashDistributionTypeService.SaveCashDistributionType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CashDistributionType cashDistributionType) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(cashDistributionType);
			return errors;
		}
	}
}