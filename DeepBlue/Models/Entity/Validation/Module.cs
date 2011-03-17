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
			[StringLength(50), Required]
			public global::System.String ModuleName {
				get;
				set;
			}
			#endregion
		}

		public MODULE(IModuleService moduleservice)
			: this() {
				this.moduleService = moduleservice;
		}

		public MODULE() {
		}

		private IModuleService _ModuleService;
		public IModuleService moduleService {
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
			var createmodule = this;
			IEnumerable<ErrorInfo> errors = Validate(createmodule);
			if (errors.Any()) {
				return errors;
			}
			moduleService.SaveModule(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(MODULE createmodule) {
			IEnumerable<ErrorInfo> errors = ValidationHelper.Validate(createmodule);
			return errors;
		}
	}
}