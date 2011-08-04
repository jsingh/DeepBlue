using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(ModuleMD))]
	public partial class MODULE {
		public class ModuleMD {
			#region Primitive Properties
			[Required(ErrorMessage = "Module is required")]
			[StringLength(50, ErrorMessage = "Module Name must be under 50 characters.")]
			public global::System.String ModuleName {
				get;
				set;
			}
			#endregion
		}

		public MODULE(IModuleService moduleService)
			: this() {
				this.ModuleService = moduleService;
		}

		public MODULE() {
		}

		private IModuleService _ModuleService;
		public IModuleService ModuleService {
			get {
				if (_ModuleService == null) {
					_ModuleService = new ModuleService();
				}
				return _ModuleService;
			}
			set {
				_ModuleService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			ModuleService.SaveModule(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(MODULE createmodule) {
			return ValidationHelper.Validate(createmodule);
		}
	}
}