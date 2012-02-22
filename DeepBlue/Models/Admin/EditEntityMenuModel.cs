using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Models.Admin {
	public class EditEntityMenuModel {

		public EditEntityMenuModel() {
			EntityMenuID = 0;
			MenuID = 0;
			DisplayName = string.Empty;
		}

		public int EntityMenuID { get; set; }

		[Required(ErrorMessage = "Menu is required")]
		[Range((int)ConfigUtil.IDStartRange, int.MaxValue, ErrorMessage = "Menu is required")]
		public int MenuID {
			get;
			set;
		}

		[Required(ErrorMessage = "DisplayName is required")]
		[StringLength(50, ErrorMessage = "DisplayName must be under 50 characters.")]
		public string DisplayName {
			get;
			set;
		}

	}
}