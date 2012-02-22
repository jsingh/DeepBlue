using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Admin {
	public class EditMenuModel {

		public EditMenuModel() {
			MenuID = 0;
			DisplayName = string.Empty;
			URL = string.Empty;
			Title = string.Empty;
		}

		public int MenuID { get; set; }

		[Required(ErrorMessage = "DisplayName is required")]
		[StringLength(50, ErrorMessage = "DisplayName must be under 50 characters.")]
		public string DisplayName {
			get;
			set;
		}

		public string ParentMenuName { get; set; }

		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "ParentMenu is required")]
		public int? ParentMenuID {
			get;
			set;
		}

		[StringLength(500, ErrorMessage = "URL must be under 500 characters.")]
		public string URL {
			get;
			set;
		}

		[StringLength(250, ErrorMessage = "Title must be under 250 characters.")]
		public string Title {
			get;
			set;
		}

	}
}