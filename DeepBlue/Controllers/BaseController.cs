using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Entity;
using System.Text;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers {
	public class BaseController : Controller {

		protected override void OnAuthorization(AuthorizationContext filterContext) {
			//System entity 
			string controllerName = Convert.ToString(this.ValueProvider.GetValue("controller").RawValue);
			string actionName = Convert.ToString(this.ValueProvider.GetValue("action").RawValue);
			if (controllerName != "Account") {
				if (Authentication.CurrentUser == null || Authentication.CurrentEntity == null) {
					RedirectLogOn(filterContext);
				}
				else if(controllerName != "Home" && controllerName != "Admin") {
					if (Authentication.IsSystemEntityUser) {
						RedirectLogOn(filterContext);
					}
				}
			}
			base.OnAuthorization(filterContext);
		}

		private void RedirectLogOn(AuthorizationContext filterContext) {
			filterContext.Result = RedirectToAction("LogOn", "Account");
		}

		protected override void OnException(ExceptionContext filterContext) {
			base.OnException(filterContext);

			Log log = new Log();
			if (Authentication.CurrentEntity != null) {
				log.EntityID = Authentication.CurrentEntity.EntityID;
			}

			if (Authentication.CurrentUser != null) {
				log.UserID = Authentication.CurrentUser.UserID;
			}

			log.LogTypeID = (int)DeepBlue.Models.Admin.Enums.LogType.Error;
			log.LogText = filterContext.Exception.Message;
			log.Controller = this.ValueProvider.GetValue("controller").RawValue.ToString();
			log.Action = this.ValueProvider.GetValue("action").RawValue.ToString();

			string qs = System.Web.HttpContext.Current.Request.ServerVariables["QUERY_STRING"].ToString();

			if (qs != null) {
				log.QueryString = qs;
			}

			if (filterContext.RequestContext.HttpContext.Request.UserAgent != null) {
				log.UserAgent = filterContext.RequestContext.HttpContext.Request.UserAgent;
			}

			StringBuilder sb = new StringBuilder();
			sb.Append("ExceptionType: ").Append(filterContext.Exception.GetType().FullName).Append(Environment.NewLine);
			sb.Append("PATH_INFO: ").Append(System.Web.HttpContext.Current.Request.ServerVariables["PATH_INFO"].ToString()).Append(Environment.NewLine);
			sb.Append("REMOTE_ADDR: ").Append(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString()).Append(Environment.NewLine);
			if (filterContext.Exception.InnerException != null) {
				sb.Append("InnerException: ").Append(filterContext.Exception.InnerException.GetType().FullName).Append(filterContext.Exception.InnerException.Message).Append(Environment.NewLine);
			}
			try {
				sb.Append("StackTrace: " + filterContext.Exception.StackTrace);
			}
			catch (Exception exc) {
				System.Diagnostics.Trace.WriteLine("The following error failed to be logged:" + exc.StackTrace.ToString());
				sb.Append("The following error failed to be logged: ").Append(exc.StackTrace.ToString()).Append(Environment.NewLine);
			}
			log.AdditionalDetail = sb.ToString();

			Logger.Write(log);

		}

	}
}
