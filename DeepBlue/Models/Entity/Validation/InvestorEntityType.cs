using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(InvestorEntityTypeMD))]
	public partial class InvestorEntityType {
		public class InvestorEntityTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Investor Entity Type Name is required")]
			[StringLength(20, ErrorMessage = "Investor Entity Type Name must be under 20 characters.")]
			public global::System.String InvestorEntityTypeName {
				get;
				set;
			}
			#endregion
		}

		public InvestorEntityType(IInvestorEntityTypeService investorEntityService)
			: this() {
			this.InvestorEntityTypeService = investorEntityService;
		}

		public InvestorEntityType() {
		}

		private IInvestorEntityTypeService _InvestorEntityService;
		public IInvestorEntityTypeService InvestorEntityTypeService {
			get {
				if (_InvestorEntityService == null) {
					_InvestorEntityService = new InvestorEntityTypeService();
				}
				return _InvestorEntityService;
			}
			set {
				_InvestorEntityService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			InvestorEntityTypeService.SaveInvestorEntityType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(InvestorEntityType investorEntityType) {
			return ValidationHelper.Validate(investorEntityType);
		}
	}
}