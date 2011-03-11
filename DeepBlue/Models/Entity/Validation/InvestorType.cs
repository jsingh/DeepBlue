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
			[Required]
			[Range((int)(int)ConfigUtil.EntityIDStartRange, int.MaxValue)]
			public global::System.Int32 EntityID {
				get;
				set;
			}
			[StringLength(100), Required]
			public global::System.String InvestorTypeName {
				get;
				set;
			}
			#endregion
		}

		public InvestorType(IInvestorTypeService investorService)
			: this() {
			this.InvestorTypeService = investorService;
		}

		public InvestorType() {
		}

		private IInvestorTypeService _investorService;
		public IInvestorTypeService InvestorTypeService {
			get {
				if (_investorService == null) {
					_investorService = new InvestorTypeService();
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
			InvestorTypeService.SaveInvestorType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(InvestorType investorEntityType) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(investorEntityType);
			return errors;
		}
	}
}