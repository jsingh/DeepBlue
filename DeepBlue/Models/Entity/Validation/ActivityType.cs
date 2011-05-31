using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(ActivityTypeMD))]
	public partial class ActivityType {
		public class ActivityTypeMD {
			#region Primitive Properties
			[Required(ErrorMessage = "Name is required")]
			[StringLength(100, ErrorMessage = "Name must be under 100 characters.")]
			public global::System.String Name {
				get;
				set;
			}
			#endregion
		}

		public ActivityType(IActivityTypeService activityTypeService)
			: this() {
			this.ActivityTypeService = activityTypeService;
		}

		public ActivityType() {
		}

		private IActivityTypeService _activityTypeService;
		public IActivityTypeService ActivityTypeService {
			get {
				if (_activityTypeService == null) {
					_activityTypeService = new ActivityTypeService();
				}
				return _activityTypeService;
			}
			set {
				_activityTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var activityType = this;
			IEnumerable<ErrorInfo> errors = Validate(activityType);
			if (errors.Any()) {
				return errors;
			}
			ActivityTypeService.SaveActivityType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(ActivityType activityType) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(activityType);
			return errors;
		}
	}
}