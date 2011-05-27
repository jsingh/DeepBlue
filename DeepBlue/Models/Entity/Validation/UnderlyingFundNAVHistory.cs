﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(UnderlyingFundNAVHistoryMD))]
	public partial class UnderlyingFundNAVHistory {
		public class UnderlyingFundNAVHistoryMD {
			#region Primitive Properties
			[Required(ErrorMessage = "UnderlyingFundNAVID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "UnderlyingFundNAVID is required")]
			public global::System.Int32 UnderlyingFundNAVID {
				get;
				set;
			}

			[Required(ErrorMessage = "FundNAV is required")]
			[Range(typeof(decimal), "1", "79228162514264337593543950335", ErrorMessage = "FundNAV is required")]
			public global::System.Decimal FundNAV {
				get;
				set;
			}

			[Required(ErrorMessage = "FundNAVDate is required")]
			[DateRange()]
			public global::System.DateTime FundNAVDate {
				get;
				set;
			}
			#endregion
		}

		public UnderlyingFundNAVHistory(IUnderlyingFundNAVHistoryService underlyingFundNAVHistoryService)
			: this() {
			this.UnderlyingFundNAVHistoryService = underlyingFundNAVHistoryService;
		}

		public UnderlyingFundNAVHistory() {
		}

		private IUnderlyingFundNAVHistoryService _underlyingFundNAVHistoryService;
		public IUnderlyingFundNAVHistoryService UnderlyingFundNAVHistoryService {
			get {
				if (_underlyingFundNAVHistoryService == null) {
					_underlyingFundNAVHistoryService = new UnderlyingFundNAVHistoryService();
				}
				return _underlyingFundNAVHistoryService;
			}
			set {
				_underlyingFundNAVHistoryService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var underlyingFundNAVHistory = this;
			IEnumerable<ErrorInfo> errors = Validate(underlyingFundNAVHistory);
			if (errors.Any()) {
				return errors;
			}
			UnderlyingFundNAVHistoryService.SaveUnderlyingFundNAVHistory(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(UnderlyingFundNAVHistory underlyingFundNAVHistory) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(underlyingFundNAVHistory);
			return errors;
		}
	}
}