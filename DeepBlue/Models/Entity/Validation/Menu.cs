using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DeepBlue.Helpers;

namespace DeepBlue.Models.Entity {
	[MetadataType(typeof(MenuMD))]
	public partial class Menu {
		public class MenuMD {

			#region Primitive Properties
			[Required(ErrorMessage = "DisplayName is required")]
			[StringLength(50, ErrorMessage = "DisplayName must be under 50 characters.")]
			public global::System.String DisplayName {
				get;
				set;
			}

			[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "ParentMenuID is required")]
			public Nullable<global::System.Int32> ParentMenuID {
				get;
				set;
			}

			[StringLength(500, ErrorMessage = "URL must be under 500 characters.")]
			public global::System.String URL {
				get;
				set;
			}

			[StringLength(250, ErrorMessage = "Title must be under 250 characters.")]
			public global::System.String Title {
				get;
				set;
			}

			#endregion
		}
		public Menu(IMenuService menuService)
			: this() {
			this.MenuService = menuService;
		}

		public Menu() {
		}

		private IMenuService _MenuService;
		public IMenuService MenuService {
			get {
				if (_MenuService == null) {
					_MenuService = new MenuService();
				}
				return _MenuService;
			}
			set {
				_MenuService = value;
			}
		}

		public IEnumerable<ErrorInfo> Save() {
			IEnumerable<ErrorInfo> errors = Validate(this);
			if (errors.Any()) {
				return errors;
			}
			MenuService.SaveMenu(this);
			return null;
		}

		private IEnumerable<ErrorInfo> Validate(Menu menu) {
			return ValidationHelper.Validate(menu);
		}
	}
}