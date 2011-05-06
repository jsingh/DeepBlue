using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DeepBlue.Helpers;

namespace DeepBlue {
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication {

	 

		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);


			routes.MapRoute(
			"New",
			"{Id}",
				new { controller = "Transaction", action = "New" },
				new { Id = @"\d+" }
			);

			routes.MapRoute(
					"List", // Route name
					"{controller}/{action}/{id}", // URL with parameters
					new { controller = "Admin", action = "List", id = @"\d+" } // Parameter defaults
				);

		}

		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();
			RegisterRoutes(RouteTable.Routes);
			DataAnnotationsModelValidatorProvider.RegisterAdapter(
															   typeof(RemoteUID_Attribute),
															   typeof(RemoteAttributeAdapter));
		}
	}
}