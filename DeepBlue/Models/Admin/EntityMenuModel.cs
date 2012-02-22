using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Admin {
	public class EntityMenuModel {

		public int EntityMenuID { get; set; }

		public int MenuID { get; set; }

		public int? ParentMenuID { get; set; }

		public string MenuName { get; set; }

		public string DisplayName { get; set; }

		public string Title { get; set; }

		public string URL { get; set; }

		public int SortOrder { get; set; }
	}
}