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

		public EquitySplit(IEquitySplitService equitySplitservice)
			: this() {
			this.equitySplitService = equitySplitservice;
		}

		public EquitySplit() {
		}

		private IEquitySplitService _EquitySplitService;
		public IEquitySplitService equitySplitService {
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
			var equitySplit = this;
			IEnumerable<ErrorInfo> errors = Validate(equitySplit);
			if (errors.Any()) {
				return errors;
			}
			equitySplitService.SaveEquitySplit(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(EquitySplit equitySplit) {
			return ValidationHelper.Validate(equitySplit);
		}
	}
}