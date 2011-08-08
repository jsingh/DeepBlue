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

			[Required(ErrorMessage = "Enabled is required")]
			public global::System.Boolean Enabled {
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

		private IActivityTypeService _ActivityTypeService;
		public IActivityTypeService ActivityTypeService {
			get {
				if (_ActivityTypeService == null) {
					_ActivityTypeService = new ActivityTypeService();
				}
				return _ActivityTypeService;
			}
			set {
				_ActivityTypeService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			ActivityTypeService.SaveActivityType(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(ActivityType activityType) {
			return ValidationHelper.Validate(activityType);
		}
	}
}