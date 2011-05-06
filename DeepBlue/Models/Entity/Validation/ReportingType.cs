using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(ReportingTypeMD))]
	public partial class ReportingType {
		public class ReportingTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Reporting is required")]
			[StringLength(100, ErrorMessage = "Reporting Name must be under 100 characters.")]
			public global::System.String Reporting {
				get;
				set;
			}

			[Required(ErrorMessage = "Created Date is required")]
			[DateRange(ErrorMessage = "Created Date is required")]
			public global::System.DateTime CreatedDate {
				get;
				set;
			}

			[Required(ErrorMessage = "CreatedBy is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "CreatedBy is required")]
			public global::System.Int32 CreatedBy {
				get;
				set;
			}

			[Required(ErrorMessage = "Last Updated Date is required")]
			[DateRange(ErrorMessage = "Last Updated Date is required")]
			public global::System.DateTime LastUpdatedDate {
				get;
				set;
			}

			[Required(ErrorMessage = "LastUpdatedBy is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "LastUpdatedBy is required")]
			public global::System.Int32 LastUpdatedBy {
				get;
				set;
			}


			#endregion
		}

		public ReportingType(IReportingTypeService reportingtypeservice)
			: this() {
				this.reportingtypeService = reportingtypeservice;
		}

		public ReportingType() {
		}

		private IReportingTypeService _ReportingTypeService;
		public IReportingTypeService reportingtypeService {
			get {
				if (_ReportingTypeService == null) {
					_ReportingTypeService = new ReportingTypeService();
				}
				return _ReportingTypeService;
			}
			set {
				_ReportingTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var createreportingtype = this;
			IEnumerable<ErrorInfo> errors = Validate(createreportingtype);
			if (errors.Any()) {
				return errors;
			}
			reportingtypeService.SaveReportingType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(ReportingType reportingtype) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(reportingtype);
			return errors;
		}
	}
}