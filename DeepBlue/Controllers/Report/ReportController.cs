using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Report;
using DeepBlue.Helpers;
using DeepBlue.Controllers.CapitalCall;
using DeepBlue.Models.Entity;

namespace DeepBlue.Controllers.Report {
	public class ReportController : Controller {

		public IReportRepository ReportRepository { get; set; }

		public ICapitalCallRepository CapitalCallRepository { get; set; }

		public ReportController()
			: this(new ReportRepository(), new CapitalCallRepository()) {
		}

		public ReportController(IReportRepository reportRepository, ICapitalCallRepository capitalCallRepository) {
			ReportRepository = reportRepository;
			CapitalCallRepository = capitalCallRepository;
		}

		#region Cash Distribution Summary
		public ActionResult CashDistributionSummary() {
			ViewData["MenuName"] = "Reports";
			ViewData["PageName"] = "CashDistributionSummary";
			CashDistributionSummaryModel model = new CashDistributionSummaryModel();
			model.CapitalDistributions = SelectListFactory.GetEmptySelectList();
			return View(model);
		}

		public JsonResult DistributionSummaryList(int fundId, int capitalDistributionlId) {
			JsonResult jsonResult = null;
			if (fundId > 0 && capitalDistributionlId > 0) {
				CapitalDistribution distribution = ReportRepository.FindCapitalDistribution(capitalDistributionlId);
				CashDistributionReportDetail detail = new CashDistributionReportDetail();
				if (distribution != null) {
					detail.FundName = distribution.Fund.FundName;
					detail.TotalDistributionAmount = FormatHelper.CurrencyFormat(distribution.DistributionAmount);
					detail.DistributionDate = distribution.CapitalDistributionDate.ToString("MM/dd/yyyy");
					detail.RepayManFees = "$0";
					detail.WithCarryAmount = "$0";
					detail.Items = ReportRepository.DistributionLineItems(fundId, capitalDistributionlId);
				}
				jsonResult = Json(detail, JsonRequestBehavior.AllowGet);
			}
			return jsonResult;
		}

		public JsonResult GetCapitalDistributions(int fundId) {
			List<Models.Entity.CapitalDistribution> capitalCallDistributions = CapitalCallRepository.GetCapitalDistributions(fundId);
			List<SelectListItem> selectLists = new List<SelectListItem>();
			foreach (var distribution in capitalCallDistributions) {
				selectLists.Add(new SelectListItem { Selected = false, Text = distribution.DistributionNumber + "# (" + distribution.CapitalDistributionDate.ToString("MM/dd/yyyy") + ")", Value = distribution.CapitalDistributionID.ToString() });
			}
			return Json(selectLists, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region Capital Call Summary
		public ActionResult CapitalCallSummary() {
			ViewData["MenuName"] = "Reports";
			ViewData["PageName"] = "CapitalCallSummary";
			CapitalCallSummaryModel model = new CapitalCallSummaryModel();
			model.CapitalCalls = SelectListFactory.GetEmptySelectList();
			return View(model);
		}

		public JsonResult CapitalCallSummaryList(int fundId, int capitalCallId) {
			JsonResult jsonResult = null;
			if (fundId > 0 && capitalCallId > 0) {
				Models.Entity.CapitalCall capitalCall = ReportRepository.FindCapitalCall(capitalCallId);
				CapitalCallReportDetail detail = new CapitalCallReportDetail();
				if (capitalCall != null) {
					detail.AmountForInv = FormatHelper.CurrencyFormat(capitalCall.InvestmentAmount);
					detail.CapitalCallDueDate = capitalCall.CapitalCallDueDate.ToString("MM/dd/yyyy");
					detail.ExistingInv = FormatHelper.CurrencyFormat(capitalCall.ExistingInvestmentAmount);
					detail.FundName = capitalCall.Fund.FundName;
					detail.Items = ReportRepository.CapitalCallLineItems(fundId, capitalCallId);
					detail.NewInv = FormatHelper.CurrencyFormat(capitalCall.NewInvestmentAmount);
					detail.TotalCapitalCall = FormatHelper.CurrencyFormat(capitalCall.CapitalAmountCalled);
					detail.TotalExpenses = FormatHelper.CurrencyFormat(capitalCall.FundExpenses);
					detail.TotalManagementFees = FormatHelper.CurrencyFormat(capitalCall.ManagementFees);
				}
				jsonResult = Json(detail, JsonRequestBehavior.AllowGet);
			}
			return jsonResult;
		}

		public JsonResult GetCapitalCalls(int fundId) {
			List<Models.Entity.CapitalCall> capitalCalls = CapitalCallRepository.GetCapitalCalls(fundId);
			List<SelectListItem> selectLists = new List<SelectListItem>();
			foreach (var capitalCall in capitalCalls) {
				selectLists.Add(new SelectListItem { Selected = false, Text = capitalCall.CapitalCallNumber + "# (" + capitalCall.CapitalCallDueDate.ToString("MM/dd/yyyy") + ")", Value = capitalCall.CapitalCallID.ToString() });
			}
			return Json(selectLists, JsonRequestBehavior.AllowGet);
		}
		#endregion
	}
}
