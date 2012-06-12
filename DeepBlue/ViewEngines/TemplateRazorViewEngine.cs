/*
using System;
using System.Web.Mvc;

namespace DeepBlue
{
	
    public class TemplateRazorViewEngine : TemplateBuildManagerViewEngine
    {
        public TemplateRazorViewEngine()
        {
            AreaViewLocationFormats = new[] {
                                                "~/Areas/{2}/Views/templates/{3}/{1}/{0}.cshtml",
                                                "~/Areas/{2}/Views/templates/{3}/{1}/{0}.vbhtml",
                                                "~/Areas/{2}/Views/templates/{3}/Shared/{0}.cshtml",
                                                "~/Areas/{2}/Views/templates/{3}/Shared/{0}.vbhtml",

                                                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                                                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                                "~/Areas/{2}/Views/Shared/{0}.vbhtml"
                                            };

            AreaMasterLocationFormats = new[] {
                                                  "~/Areas/{2}/Views/templates/{3}/{1}/{0}.cshtml",
                                                  "~/Areas/{2}/Views/templates/{3}/{1}/{0}.vbhtml",
                                                  "~/Areas/{2}/Views/templates/{3}/Shared/{0}.cshtml",
                                                  "~/Areas/{2}/Views/templates/{3}/Shared/{0}.vbhtml",

                                                  "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                  "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                                                  "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                                  "~/Areas/{2}/Views/Shared/{0}.vbhtml"
                                              };

            AreaPartialViewLocationFormats = new[] {
                                                       "~/Areas/{2}/Views/templates/{3}/{1}/{0}.cshtml",
                                                       "~/Areas/{2}/Views/templates/{3}/{1}/{0}.vbhtml",
                                                       "~/Areas/{2}/Views/templates/{3}/Shared/{0}.cshtml",
                                                       "~/Areas/{2}/Views/templates/{3}/Shared/{0}.vbhtml",

                                                       "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                       "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                                                       "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                                       "~/Areas/{2}/Views/Shared/{0}.vbhtml"
                                                   };

            ViewLocationFormats = new[] {
                                            "~/Views/Templates/{2}/{1}/{0}.cshtml",
                                            "~/Views/Templates/{2}/{1}/{0}.vbhtml",
                                            "~/Views/Templates/{2}/Shared/{0}.cshtml",
                                            "~/Views/Templates/{2}/Shared/{0}.vbhtml",

                                            "~/Views/{1}/{0}.cshtml",
                                            "~/Views/{1}/{0}.vbhtml",
                                            "~/Views/Shared/{0}.cshtml",
                                            "~/Views/Shared/{0}.vbhtml"
                                        };

            MasterLocationFormats = new[] {
                                              "~/Views/Templates/{2}/{1}/{0}.cshtml",
                                              "~/Views/Templates/{2}/{1}/{0}.vbhtml",
                                              "~/Views/Templates/{2}/Shared/{0}.cshtml",
                                              "~/Views/Templates/{2}/Shared/{0}.vbhtml",

                                              "~/Views/{1}/{0}.cshtml",
                                              "~/Views/{1}/{0}.vbhtml",
                                              "~/Views/Shared/{0}.cshtml",
                                              "~/Views/Shared/{0}.vbhtml"
                                          };

            PartialViewLocationFormats = new[] {
                                                   "~/Views/Templates/{2}/{1}/{0}.cshtml",
                                                   "~/Views/Templates/{2}/{1}/{0}.vbhtml",
                                                   "~/Views/Templates/{2}/Shared/{0}.cshtml",
                                                   "~/Views/Templates/{2}/Shared/{0}.vbhtml",

                                                   "~/Views/{1}/{0}.cshtml",
                                                   "~/Views/{1}/{0}.vbhtml",
                                                   "~/Views/Shared/{0}.cshtml",
                                                   "~/Views/Shared/{0}.vbhtml"
                                               };

            ViewStartFileExtensions = new[] { "cshtml", "vbhtml", };
        }

        public string[] ViewStartFileExtensions { get; set; }

	 
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            //return new RazorView(controllerContext, partialPath, null, false, ViewStartFileExtensions);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            //return new RazorView(controllerContext, viewPath, masterPath, true, ViewStartFileExtensions);
        }

        protected override bool IsValidCompiledType(ControllerContext controllerContext, string virtualPath, Type compiledType)
        {
			//return typeof(System.Web.Mvc.WebViewPage).IsAssignableFrom(compiledType);
        }
		 
    }

}
*/