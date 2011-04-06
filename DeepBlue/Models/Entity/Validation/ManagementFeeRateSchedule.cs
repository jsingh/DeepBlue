using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;
using DeepBlue;


namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(ManagementFeeRateScheduleMD))]
	public partial class ManagementFeeRateSchedule {
		public class ManagementFeeRateScheduleMD {
			#region Primitive Properties

			[Required]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue)]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			#endregion
		}

		public ManagementFeeRateSchedule(IManagementFeeRateScheduleService managementFeeRateScheduleService)
			: this() {
			this.ManagementFeeRateScheduleService = managementFeeRateScheduleService;
		}

		public ManagementFeeRateSchedule() {
		}

		private IManagementFeeRateScheduleService _managementFeeRateScheduleService;
		public IManagementFeeRateScheduleService ManagementFeeRateScheduleService {
			get {
				if (_managementFeeRateScheduleService == null) {
					_managementFeeRateScheduleService = new ManagementFeeRateScheduleService();
				}
				return _managementFeeRateScheduleService;
			}
			set {
				_managementFeeRateScheduleService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			var managementFeeRateSchedule = this;
			IEnumerable<ErrorInfo> errors = Validate(managementFeeRateSchedule);
			if (errors.Any()) {
				return errors;
			}
			ManagementFeeRateScheduleService.SaveManagementFeeRateSchedule(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(ManagementFeeRateSchedule managementFeeRateSchedule) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(managementFeeRateSchedule);
			foreach (ManagementFeeRateScheduleTier tier in managementFeeRateSchedule.ManagementFeeRateScheduleTiers) {
				errors = errors.Union(ValidationHelper.Validate(tier));
			}
			return errors;
		}
	}

	[MetadataType(typeof(ManagementFeeRateScheduleTierMD))]
	public partial class ManagementFeeRateScheduleTier {
		public class ManagementFeeRateScheduleTierMD {
			#region Primitive Properties
			[Required]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "MultiplierType is required")]
			public global::System.Int32 MultiplierTypeID {
				get;
				set;
			}
			#endregion
		}
	}
}