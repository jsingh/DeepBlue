using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Admin;

namespace DeepBlue.Controllers {
	public class HomeController : BaseController {
		
		//
		// GET: /Home/
		public ActionResult Index() {
			ViewData["MenuName"] = "DealManagement";
			return View();
		}

		[ChildActionOnly]
		public ActionResult Menu() {
			ViewData = this.ControllerContext.ParentActionViewContext.ViewData;

			List<MenuModel> menu = new List<MenuModel>();

			/* Deal Management */

			menu.Add(new MenuModel {

				Name = "DealManagement",

				DisplayName = "DEALS",

				HtmlAttributes = new { id = "investmentmnu" },

				Childs = new List<MenuModel> { 
				
				new MenuModel { DisplayName = "Create New Deal", Name = "CreateNewDeal", ActionName = "New", ControllerName = "Deal" },

				new MenuModel {	DisplayName = "Modify Deal", Name = "DealList", ActionName = "List", ControllerName = "Deal" },

				new MenuModel { DisplayName = "Close Deal", Name = "CloseDeal", ActionName = "List", ControllerName = "Deal", RouteValues = new { @isCloseDeal = true } },

				new MenuModel {	DisplayName = "Deal Report", Name = "DealReport", ActionName = "Report", ControllerName = "Deal" }

			   }

			});

			/* Activities Management */

			menu.Add(new MenuModel {

				Name = "ActivitiesManagement",

				DisplayName = "ACTIVITIES",

				HtmlAttributes = new { id = "investmentmnu" },

				Childs = new List<MenuModel> { 
				 	new MenuModel { DisplayName = "Activities", Name = "Activities", ActionName = "Activities", ControllerName = "Deal" },
			   }

			});

			/* Asset Management */

			menu.Add(new MenuModel {

				Name = "AssetManagement",

				DisplayName = "ASSET LIBRARIES",

				HtmlAttributes = new { id = "docmnu" },

				Childs = new List<MenuModel> {
					new MenuModel { DisplayName = "Direct Library", Name = "DirectLibrary", ActionName = "Directs", ControllerName = "Deal" },
					
					new MenuModel { DisplayName = "Underlying Funds Library", Name = "UnderlyingFundsLibrary", ActionName = "UnderlyingFunds", ControllerName = "Deal" },

					new MenuModel { DisplayName = "Add New", Name = "AddNew", HtmlAttributes = new { @onclick = "javascript:deepBlue.redirectLibrary();" } },
				}

			});

			/* Fund Management */

			menu.Add(new MenuModel {

				Name = "FundManagement",

				DisplayName = "AMBERBROOK FUNDS",

				HtmlAttributes = new { id = "fundmnu" },

				Childs = new List<MenuModel> {
					new MenuModel { DisplayName = "Capital Call", Name = "CapitalCall", ActionName = "New", ControllerName = "CapitalCall" },

					new MenuModel { DisplayName = "Modify Capital Call", Name = "ModifyCapitalCall", ActionName = "ModifyCapitalCall", ControllerName = "CapitalCall" },

					new MenuModel { DisplayName = "Capital Distribution", Name = "CapitalDistribution", ActionName = "NewCapitalDistribution", ControllerName = "CapitalCall" },

					new MenuModel { DisplayName = "Modify Capital Distribution", Name = "ModifyCapitalDistribution", ActionName = "ModifyCapitalDistribution", ControllerName = "CapitalCall" },

					new MenuModel { DisplayName = "Detail", Name = "Detail", ActionName = "Detail", ControllerName = "CapitalCall" },

					new MenuModel { DisplayName = "Amberbrook Fund Library", Name = "AmberbrookFundLibrary", ActionName = "Index", ControllerName = "Fund" },

				}

			});

			/* Investor Management */

			menu.Add(new MenuModel {

				Name = "InvestorManagement",

				DisplayName = "INVESTORS",

				HtmlAttributes = new { id = "invmnu" },

				Childs = new List<MenuModel> {
					new MenuModel { DisplayName = "New Investor Setup", Name = "NewInvestorSetup", ActionName = "New", ControllerName = "Investor" },

					new MenuModel { DisplayName = "Update Investor Information", Name = "UpdateInvestorInformation", ActionName = "Edit", ControllerName = "Investor" },

					new MenuModel { DisplayName = "Investor Commitment", Name = "InvestorCommitment", ActionName = "New", ControllerName = "Transaction" },

					new MenuModel { DisplayName = "Investor Library", Name = "InvestorLibrary", ActionName = "Library", ControllerName = "Investor" },
				}

			});

			/* Accounting Management */

			menu.Add(new MenuModel {

				Name = "AccountingManagement",

				DisplayName = "ACCOUNTING",

				HtmlAttributes = new { id = "accmnu" },

				Childs = new List<MenuModel> {

				}

			});

			/* Admin Managemnet */

			menu.Add(new MenuModel {

				Name = "AdminManagement",

				DisplayName = "ADMIN",

				HtmlAttributes = new { id = "admmnu" },

				Childs = new List<MenuModel> {
					new MenuModel { DisplayName = "Investor Management", Name = "InvestorManagement", ActionName = "EntityType", ControllerName = "Admin" },

					new MenuModel { DisplayName = "Custom Field Management", Name = "CustomFieldManagement", ActionName = "CustomField", ControllerName = "Admin" },

					new MenuModel { DisplayName = "Deal Management", Name = "DealManagement", ActionName = "PurchaseType", ControllerName = "Admin" },

					new MenuModel { DisplayName = "Document Management", Name = "DocumentManagement", ActionName = "DocumentType", ControllerName = "Admin" },

					new MenuModel { DisplayName = "User Management", Name = "UserManagement", ActionName = "Users", ControllerName = "Admin", IsAdmin = true },

					new MenuModel { DisplayName = "Export Excel", Name = "ExportExcel", ActionName = "Export", ControllerName = "Admin" },
				}

			});

			/* Report Managemnet */

			menu.Add(new MenuModel {

				Name = "ReportManagement",

				DisplayName = "REPORTS",

				HtmlAttributes = new { id = "repmnu" },

				Childs = new List<MenuModel> {
					new MenuModel { DisplayName = "Deal Detail", Name = "DealDetail", ActionName = "DealDetail", ControllerName = "Report" },

					new MenuModel { DisplayName = "Deal Origination", Name = "DealOrigination", ActionName = "DealOrigination", ControllerName = "Report" },

					new MenuModel { DisplayName = "Fund Break Down", Name = "FundBreakDown", ActionName = "FundBreakDown", ControllerName = "Report" },

					new MenuModel { DisplayName = "Cash Distribution Summary", Name = "CashDistributionSummary", ActionName = "CashDistributionSummary", ControllerName = "Report" },

					new MenuModel { DisplayName = "Capital Call Summary", Name = "CapitalCallSummary", ActionName = "CapitalCallSummary", ControllerName = "Report" },

					new MenuModel { DisplayName = "Distribution", Name = "Distribution", ActionName = "Distribution", ControllerName = "Report" },

					new MenuModel { DisplayName = "Fees Expense", Name = "FeesExpense", ActionName = "FeesExpense", ControllerName = "Report" },

					new MenuModel { DisplayName = "Security Value", Name = "SecurityValue", ActionName = "SecurityValue", ControllerName = "Report" },

					new MenuModel { DisplayName = "Underlying Fund NAV", Name = "UnderlyingFundNAV", ActionName = "UnderlyingFundNAV", ControllerName = "Report" },

					new MenuModel { DisplayName = "Unfunded Capital Call Balance", Name = "UnfundedCapitalCallBalance", ActionName = "UnfundedCapitalCallBalance", ControllerName = "Report" },
				}

			});

			return View(menu);
		}
				
	}
}

