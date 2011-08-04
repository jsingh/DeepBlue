using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(EquitySplitMD))]
	public partial class EquitySplit {
		public class EquitySplitMD : CreatedByFields {
			#region Primitive Properties

			[Required(ErrorMessage = "EquityID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "EquityID is required")]
			public global::System.Int32 EquityID {
				get;
				set;
			}

			[Required(ErrorMessage = "SplitFactor is required")]
			[Range((int)1, int.MaxValue, ErrorMessage = "SplitFactor is required")]
			public global::System.Int32 SplitFactor {
				get;
				set;
			}

			[Required(ErrorMessage = "SplitDate is required")]
			[DateRange()]
			public global::System.DateTime SplitDate {
				get;
				set;
			}

			#endregion
		}

		public EquitySplit(IEquitySplitService equitySplitService)
			: this() {
			this.EquitySplitService = equitySplitService;
		}

		public EquitySplit() {
		}

		private IEquitySplitService _EquitySplitService;
		public IEquitySplitService EquitySplitService {
			get {
				if (_EquitySplitService == null) {
					_EquitySplitService = new EquitySplitService();
				}
				return _EquitySplitService;
			}
			set {
				_EquitySplitService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			EquitySplitService.SaveEquitySplit(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(EquitySplit equitySplit) {
			return ValidationHelper.Validate(equitySplit);
		}
	}
}