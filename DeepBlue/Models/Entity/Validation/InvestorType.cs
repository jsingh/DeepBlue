using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(InvestorTypeMD))]
	public partial class InvestorType {
		public class InvestorTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Investor Type Name is required")]
			[StringLength(20, ErrorMessage = "Investor Type Name must be under 20 characters.")]
			public global::System.String InvestorTypeName {
				get;
				set;
			}
			#endregion
		}

		public InvestorType(IInvestorTypeService investorTypeService)
			: this() {
			this.InvestorTypeService = investorTypeService;
		}

		public InvestorType() {
		}

		private IInvestorTypeService _InvestorTypeService;
		public IInvestorTypeService InvestorTypeService {
			get {
				if (_InvestorTypeService == null) {
					_InvestorTypeService = new InvestorTypeService();
				}
				return _InvestorTypeService;
			}
			set {
				_InvestorTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			InvestorTypeService.SaveInvestorType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(InvestorType investorType) {
			return ValidationHelper.Validate(investorType);
		}
	}
}