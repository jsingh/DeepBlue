using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(AnnualMeetingHistoryMD))]
	public partial class AnnualMeetingHistory {
		public class AnnualMeetingHistoryMD {

			#region Primitive Properties
			[Required(ErrorMessage = "IssuerID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "IssuerID is required")]
			public global::System.Int32 IssuerID {
				get;
				set;
			}

			[DateRange(ErrorMessage = "AnnualMeetingDate is required")]
			public Nullable<global::System.DateTime> AnnualMeetingDate {
				get;
				set;
			}

			#endregion
		}
		public AnnualMeetingHistory(IAnnualMeetingHistoryService annualMeetingHistoryService)
			: this() {
			this.AnnualMeetingHistoryService = annualMeetingHistoryService;
		}

		public AnnualMeetingHistory() {
		}

		private IAnnualMeetingHistoryService _AnnualMeetingHistoryService;
		public IAnnualMeetingHistoryService AnnualMeetingHistoryService {
			get {
				if (_AnnualMeetingHistoryService == null) {
					_AnnualMeetingHistoryService = new AnnualMeetingHistoryService();
				}
				return _AnnualMeetingHistoryService;
			}
			set {
				_AnnualMeetingHistoryService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			AnnualMeetingHistoryService.SaveAnnualMeetingHistory(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(AnnualMeetingHistory annualMeetingHistory) {
			return ValidationHelper.Validate(annualMeetingHistory);
		}
	}
}