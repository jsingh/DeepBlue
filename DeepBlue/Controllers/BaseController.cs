using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Entity;
using System.Text;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers
{
    public class BaseController : Controller
    {

		protected override void OnActionExecuting(ActionExecutingContext filterContext) {
			if (this.ValueProvider.GetValue("controller").RawValue.ToString() != "Account") {
				if (Authentication.CurrentUser == null || Authentication.CurrentEntity == null) {
					filterContext.Result = RedirectToAction("LogOn", "Account");
					return;
				}
			}
			//otherwise continue with action
			base.OnActionExecuting(filterContext);
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
				sb.Append("InnerException: ").Append(filterContext.Exception.InnerException.GetType().FullName).Append(filterContext.Exception.InnerException.Message)					.Append(Environment.NewLine);
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

			////If custom errors are On, do our custom error page.
			//if (filterContext.HttpContext.IsCustomErrorEnabled) {
			//    //Get the exception and the page that errored for the e-mail
			//    System.Web.HttpContext.Current.Session.Add("Exception", filterContext.Exception.ToString());
			//    System.Web.HttpContext.Current.Session.Add("ErrorPage", filterContext.HttpContext.Request.FilePath);

			//    //Tell the framework we've handled the error
			//    filterContext.ExceptionHandled = true;

			//    //Show the custom error page.
			//    this.View("../GroupApp/GenericError", new InFellowship.Controllers.Groups.SubmitIssueViewModel(BaseView.CurrentRequestID)).ExecuteResult(this.ControllerContext);
			//}
		}

    }
}
