using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity.Partial;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingFundStockDistributionMD))]
	public partial class UnderlyingFundStockDistribution {
		public class UnderlyingFundStockDistributionMD {
			#region Primitive Properties
			
			[Required(ErrorMessage = "Fund is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Fund is required")]
			public global::System.Int32 FundID {
				get;
				set;
			}

			[Required(ErrorMessage = "Underlying Fund is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Underlying Fund is required")]
			public global::System.Int32 UnderlyingFundID {
				get;
				set;
			}

			[Required(ErrorMessage = "Security is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Security is required")]
			public global::System.Int32 SecurityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Security Type is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Security Type is required")]
			public global::System.Int32 SecurityTypeID {
				get;
				set;
			}
 
			#endregion
		}

		public UnderlyingFundStockDistribution(IUnderlyingFundStockDistributionService underlyingFundStockDistributionservice)
			: this() {
			this.UnderlyingFundStockDistributionService = UnderlyingFundStockDistributionService;
		}

		public UnderlyingFundStockDistribution() {
		}

		private IUnderlyingFundStockDistributionService _UnderlyingFundStockDistributionService;
		public IUnderlyingFundStockDistributionService UnderlyingFundStockDistributionService {
			get {
				if (_UnderlyingFundStockDistributionService == null) {
					_UnderlyingFundStockDistributionService = new UnderlyingFundStockDistributionService();
				}
				return _UnderlyingFundStockDistributionService;
			}
			set {
				_UnderlyingFundStockDistributionService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var underlyingFundStockDistribution = this;
			IEnumerable<ErrorInfo> errors = Validate(underlyingFundStockDistribution);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingFundStockDistributionService.SaveUnderlyingFundStockDistribution(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFundStockDistribution underlyingFundStockDistribution) {
			return ValidationHelper.Validate(underlyingFundStockDistribution);
		}
	}
}