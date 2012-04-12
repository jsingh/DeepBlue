using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.Routing;
using DeepBlue.Helpers;
using DeepBlue.Models.Entity;
using System.Web.Security;

namespace DeepBlue {
	 
	public class MvcApplication : System.Web.HttpApplication {

		public static void RegisterRoutes(RouteCollection routes) {

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
		 
			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
		}

		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();
			RegisterRoutes(RouteTable.Routes);
			DataAnnotationsModelValidatorProvider.RegisterAdapter(
															   typeof(RemoteUID_Attribute),
															   typeof(RemoteAttributeAdapter));
			RegisterViewEngine(ViewEngines.Engines);
		}

		public static void RegisterViewEngine(ViewEngineCollection viewEngines) {
			// We do not need the default view engine
			viewEngines.Clear();

			var templateableRazorViewEngine = new TemplateWebformViewEngine {
				CurrentTemplate = httpContext => httpContext.Request["template"] as string ??  string.Empty
			};

			viewEngines.Add(templateableRazorViewEngine);
		}
	}
}