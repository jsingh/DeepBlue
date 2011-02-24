using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeepBlue.Controllers {
	public class BaseController : Controller {
		private static int _CurrentEntityID = 2;
		public static int CurrentEntityID {
			get { return _CurrentEntityID; }
		}
	}
}
