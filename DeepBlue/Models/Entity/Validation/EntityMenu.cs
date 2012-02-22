using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(EntityMenuMD))]
	public partial class EntityMenu {

		public class EntityMenuMD {

			#region Primitive Properties
			[Required(ErrorMessage = "EntityID is required")]
			[Range((int)ConfigUtil.EntityIDStartRange, int.MaxValue, ErrorMessage = "EntityID is required")]
			public global::System.Int32 EntityID {
				get;
				set;
			}

			[Required(ErrorMessage = "MenuID is required")]
			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "MenuID is required")]
			public global::System.Int32 MenuID {
				get;
				set;
			}

			[Required(ErrorMessage = "DisplayName is required")]
			[StringLength(50, ErrorMessage = "DisplayName must be under 50 characters.")]
			public global::System.String DisplayName {
				get;
				set;
			}

			[Required(ErrorMessage = "SortOrder is required")]
			[Range((int)0, int.MaxValue, ErrorMessage = "SortOrder is required")]
			public global::System.Int32 SortOrder {
				get;
				set;
			}

			#endregion
		}

		public EntityMenu(IEntityMenuService entityMenuService)
			: this() {
			this.EntityMenuService = entityMenuService;
		}

		public EntityMenu() {
		}

		private IEntityMenuService _EntityMenuService;
		public IEntityMenuService EntityMenuService {
			get {
				if (_EntityMenuService == null) {
					_EntityMenuService = new EntityMenuService();
				}
				return _EntityMenuService;
			}
			set {
				_EntityMenuService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			EntityMenuService.SaveEntityMenu(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(EntityMenu entityMenu) {
			return ValidationHelper.Validate(entityMenu);
		}
	}
}