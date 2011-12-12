using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace DeepBlue.Models.Admin {
	public class MenuModel {

		public MenuModel() {
			HtmlAttributes = new { };
			RouteValues = new { };
		}

		public string Name { get; set; }

		public string DisplayName { get; set; }

		public bool IsTopMenu {
			get {
				return string.IsNullOrEmpty(this.ParentName);
			}
		}

		public string ParentName {
			get {
				return string.Empty;
			}
		}

		public bool IsAdmin { get; set; }

		public List<MenuModel> Childs { get; set; }

		public string ActionName { get; set; }

		public string ControllerName { get; set; }

		public object RouteValues { get; set; }

		public object HtmlAttributes { get; set; }

	}

	 
}