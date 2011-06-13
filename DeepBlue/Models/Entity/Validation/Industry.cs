using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(IndustryMD))]
	public partial class Industry {
		public class IndustryMD : CreatedByFields {
			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "Industry is required")]
			[StringLength(100, ErrorMessage = "Industry Name must be under 100 characters.")]
			public global::System.String Industry1 {
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