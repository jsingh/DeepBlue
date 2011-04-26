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
			CapitalDistribution distribution = ReportRepository.FindCapitalDistribution(capitalDistributionlId);
			CashDistributionReportDetail detail = new CashDistributionReportDetail();
			detail.FundName = distribution.Fund.FundName;
			detail.TotalDistributionAmount = string.Format("{0:C}", distribution.DistributionAmount);
			detail.DistributionDate = distribution.CapitalDistributionDate.ToString("MM/dd/yyyy");
			detail.RepayManFees = "$0";
			detail.WithCarryAmount = "$0";
			detail.Items = ReportRepository.DistributionLineItems(fundId, capitalDistributionlId);
			return Json(detail, JsonRequestBehavior.AllowGet);
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
			Models.Entity.CapitalCall capitalCall = ReportRepository.FindCapitalCall(capitalCallId);
			CapitalCallReportDetail detail = new CapitalCallReportDetail();
			detail.AmountForInv = string.Format("{0:C}", capitalCall.InvestmentAmount);
			detail.CapitalCallDueDate = capitalCall.CapitalCallDueDate.ToString("MM/dd/yyyy");
			detail.ExistingInv = string.Format("{0:C}", capitalCall.ExistingInvestmentAmount);
			detail.FundName = capitalCall.Fund.FundName;
			detail.Items = ReportRepository.CapitalCallLineItems(fundId, capitalCallId);
			detail.NewInv = string.Format("{0:C}", capitalCall.NewInvestmentAmount);
			detail.TotalCapitalCall = string.Format("{0:C}", capitalCall.CapitalAmountCalled);
			detail.TotalExpenses = string.Format("{0:C}", capitalCall.FundExpenses);
			detail.TotalManagementFees = string.Format("{0:C}", capitalCall.ManagementFees);
			return Json(detail, JsonRequestBehavior.AllowGet);
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
