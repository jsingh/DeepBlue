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
	public class AccountController : BaseController {

		public IAccountRepository AccountRepository { get; set; }

		public AccountController()
			: this(new AccountRepository()) {
		}

		public AccountController(IAccountRepository accountRepository) {
			AccountRepository = accountRepository;
		}

		//
		// GET: /Account/

		public ActionResult LogOn(string returnUrl) {
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}


		//
		// POST: /Account/LogOn
		[HttpPost]
		public ActionResult LogOn(FormCollection collection) {
			LogOnModel model = new LogOnModel();
			this.TryUpdateModel(model);
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
					if (IsLocalUrl(model.ReturnUrl) && model.ReturnUrl.Length > 1 && model.ReturnUrl.StartsWith("/")
						&& !model.ReturnUrl.StartsWith("//") && !model.ReturnUrl.StartsWith("/\\")) {
							return Redirect(model.ReturnUrl);
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
