using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(IndustryMD))]
	public partial class Industry {
		public class IndustryMD {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Industry is required")]
			[StringLength(100, ErrorMessage = "Industry Name must be under 100 characters.")]
			public global::System.String Industry {
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

		public Industry(IIndustryService industryservice)
			: this() {
				this.industryService = industryservice;
		}

		public Industry() {
		}

		private IIndustryService _IndustryService;
		public IIndustryService industryService {
			get {
				if (_IndustryService == null) {
					_IndustryService = new IndustryService();
				}
				return _IndustryService;
			}
			set {
				_IndustryService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var createindustry= this;
			IEnumerable<ErrorInfo> errors = Validate(createindustry);
			if (errors.Any()) {
				return errors;
			}
			industryService.SaveIndustry(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(Industry industry) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(industry);
			return errors;
		}
	}
}