using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Admin;
using System.Xml.Linq;
using System.Web.Routing;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity;
using DeepBlue.Controllers.Admin;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Data;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.IO;
using System.Text;

namespace DeepBlue.Controllers {
	public class HomeController:BaseController {
		//
		// GET: /Home/
		public ActionResult Index() {
			ViewData["MenuName"]="DealManagement";
			return RedirectToAction("Index","Fund",null);
		}

	 
	}
}

