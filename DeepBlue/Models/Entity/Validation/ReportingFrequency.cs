using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(ReportingFrequencyMD))]
	public partial class ReportingFrequency {
		public class ReportingFrequencyMD : CreatedByFields {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "ReportingFrequency is required")]
			[StringLength(100, ErrorMessage = "Reporting Frequency Name must be under 100 characters.")]
			public global::System.String ReportingFrequency1 {
				get;
				set;
			}

			#endregion
		}

		public ReportingFrequency(IReportingFrequencyService reportingfrequencyservice)
			: this() {
			this.reportingfrequencyService = reportingfrequencyservice;
		}

		public ReportingFrequency() {
		}

		private IReportingFrequencyService _ReportingFrequencyService;
		public IReportingFrequencyService reportingfrequencyService {
			get {
				if (_ReportingFrequencyService == null) {
					_ReportingFrequencyService = new ReportingFrequencyService();
				}
				return _ReportingFrequencyService;
			}
			set {
				_ReportingFrequencyService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var createreportingfrequency = this;
			IEnumerable<ErrorInfo> errors = Validate(createreportingfrequency);
			if (errors.Any()) {
				return errors;
			}
			reportingfrequencyService.SaveReportingFrequency(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(ReportingFrequency reportingfrequency) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(reportingfrequency);
			return errors;
		}
	}
}