using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Account;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using System.Web.Security;

namespace DeepBlue.Controllers.Account {
	public class AccountController : Controller {

		public IAccountRepository AccountRepository { get; set; }

		public AccountController()
			: this(new AccountRepository()) {
		}

		public AccountController(IAccountRepository accountRepository) {
			AccountRepository = accountRepository;
		}

		//
		// GET: /Account/

		public ActionResult LogOn() {
			return View();
		}


		//
		// POST: /Account/LogOn
		[HttpPost]
		public ActionResult LogOn(LogOnModel model, string returnUrl) {
			if (ModelState.IsValid) {
				bool isAuthenticated = false;
				ENTITY entity = AccountRepository.FetchUserEntity(AppSettingsHelper.CurrentEntityID);
				USER userLogin = AccountRepository.FetchUserLogin(model.UserName, entity.EntityID);
				if (userLogin != null) {
					if (userLogin.PasswordHash.ComparePassword(userLogin.PasswordSalt, model.Password)) {
						isAuthenticated = true;
					}
				}
				if (isAuthenticated) {
					Authentication.CurrentUser = userLogin;
					Authentication.CurrentEntity = entity;
					FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
					if (IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
						&& !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\")) {
						return Redirect(returnUrl);
					}
					else {
						return RedirectToAction("Index", "Home");
					}
				}
				else {
					ModelState.AddModelError("Errors", "Invalid Login");
				}
			}
			// If we got this far, something failed, redisplay form
			return View(model);
		}
 
		public ActionResult LogOff() {
			Authentication.Flush();
			FormsAuthentication.SignOut();
			return RedirectToAction("Index", "Home");
		}

		//Note: This has been copied from the System.Web.WebPages RequestExtensions class
		private bool IsLocalUrl(string url) {
			if (string.IsNullOrEmpty(url)) {
				return false;
			}

			Uri absoluteUri;
			if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri)) {
				return String.Equals(this.Request.Url.Host, absoluteUri.Host,
							StringComparison.OrdinalIgnoreCase);
			}
			else {
				bool isLocal = !url.StartsWith("http:", StringComparison.OrdinalIgnoreCase)
					&& !url.StartsWith("https:", StringComparison.OrdinalIgnoreCase)
					&& Uri.IsWellFormedUriString(url, UriKind.Relative);
				return isLocal;
			}
		}
	}
}
