using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Admin;
using System.Xml.Linq;
using System.Web.Routing;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers {
	public class HomeController : BaseController {

		//
		// GET: /Home/
		public ActionResult Index() {
			ViewData["MenuName"] = "DealManagement";
			return View();
		}

		[ChildActionOnly]
		public ActionResult Menu() {
			ViewData = this.ControllerContext.ParentActionViewContext.ViewData;
			return View(MenuHelper.GetMenus());
		}

		[ChildActionOnly]
		public ActionResult LeftMenu() {
			ViewData = this.ControllerContext.ParentActionViewContext.ViewData;
			return View(MenuHelper.GetMenus());
		}

	}
}

