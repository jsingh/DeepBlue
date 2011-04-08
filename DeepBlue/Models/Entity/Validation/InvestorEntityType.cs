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

		public InvestorEntityType(IInvestorEntityTypeService investorService)
			: this() {
			this.InvestorEntityTypeService = investorService;
		}

		public InvestorEntityType() {
		}

		private IInvestorEntityTypeService _investorService;
		public IInvestorEntityTypeService InvestorEntityTypeService {
			get {
				if (_investorService == null) {
					_investorService = new InvestorEntityTypeService();
				}
				return _investorService;
			}
			set {
				_investorService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var investor = this;
			IEnumerable<ErrorInfo> errors = Validate(investor);
			if (errors.Any()) {
				return errors;
			}
			InvestorEntityTypeService.SaveInvestorEntityType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(InvestorEntityType investorEntityType) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(investorEntityType);
			return errors;
		}
	}
}