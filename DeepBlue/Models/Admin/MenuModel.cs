using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.Routing;

namespace DeepBlue.Models.Admin {

	public class MenuModel {

		public MenuModel() {
			HtmlAttributes = new Dictionary<string, object>();
			RouteValues = new RouteValueDictionary();
			Childs = new List<MenuModel>();
			IsSystemEntity = false;
			IsOtherEntity = true;
		}

		public string Name { get; set; }

		public string DisplayName { get; set; }

		public bool IsTopMenu { get; set; }

		public bool IsAdmin { get; set; }

		public bool IsSystemEntity { get; set; }

		public bool IsOtherEntity { get; set; }

		public List<MenuModel> Childs { get; set; }

		public string ActionName { get; set; }

		public string ControllerName { get; set; }

		public RouteValueDictionary RouteValues { get; set; }

		public IDictionary<string, object> HtmlAttributes { get; set; }

	}

	 
}