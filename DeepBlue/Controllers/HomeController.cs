﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeepBlue.Controllers {
	public class HomeController : Controller {
		//
		// GET: /Home/
		public ActionResult Index() {
			ViewData["MenuName"] = "Investor";
			return View();
		}

	}
}
