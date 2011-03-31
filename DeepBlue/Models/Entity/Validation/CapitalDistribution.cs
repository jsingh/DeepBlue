using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(CapitalDistributionMD))]
	public partial class CapitalDistribution {
		public class CapitalDistributionMD {
			#region Primitive Properties
			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue)]
			public global::System.Int32 FundID {
				get;
				set;
			}
 
			[Required]
			[DateRange()]
			public global::System.DateTime CapitalDistributionDate {
				get;
				set;
			}

			[Required]
			[DateRange()]
			public global::System.DateTime CapitalDistributionDueDate {
				get;
				set;
			}


			[Required]
			[Range(1, (double)decimal.MaxValue)]
			public global::System.Decimal DistributionAmount {
				get;
				set;
			}


			[Required]
			[StringLength(12)]
			public global::System.String DistributionNumber {
				get;
				set;
			}

			#endregion
		}

		public CapitalDistribution(ICapitalDistributionService capitalCallservice)
			: this() {
			this.capitalCallservice = capitalCallservice;
		}

		public CapitalDistribution() {
		}

		private ICapitalDistributionService _capitalCallService;
		public ICapitalDistributionService capitalCallservice {
			get {
				if (_capitalCallService == null) {
					_capitalCallService = new CapitalDistributionService();
				}
				return _capitalCallService;
			}
			set {
				_capitalCallService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var capitalCall = this;
			IEnumerable<ErrorInfo> errors = Validate(capitalCall);
			if (errors.Any()) {
				return errors;
			}
			capitalCallservice.SaveCapitalDistribution(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(CapitalDistribution capitalCallclosing) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(capitalCallclosing);
			return errors;
		}
	}
}