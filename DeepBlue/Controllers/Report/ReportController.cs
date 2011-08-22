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

		public JsonResult DistributionSummaryList(FormCollection collection) {
			CashDistributionSummaryModel model = new CashDistributionSummaryModel();
			this.TryUpdateModel(model, collection);
			string error = string.Empty;
			ResultModel resultModel = new ResultModel();
			CashDistributionReportDetail detail = null;
			if (ModelState.IsValid) {
				detail = ReportRepository.FindCapitalDistribution(model.CapitalDistributionId);
			}
			else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							error += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return Json(new { Error = error, Data = detail }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetCapitalDistributions(int fundId) {
			List<Models.Entity.CapitalDistribution> capitalCallDistributions = CapitalCallRepository.GetCapitalDistributions(fundId);
			List<SelectListItem> selectLists = new List<SelectListItem>();
			foreach (var distribution in capitalCallDistributions) {
				selectLists.Add(new SelectListItem { Selected = false, Text = distribution.DistributionNumber + "# (" + distribution.CapitalDistributionDate.ToString("MM/dd/yyyy") + ")", Value = distribution.CapitalDistributionID.ToString() });
			}
			return Json(selectLists, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/ExportCapitalCallSummaryDetail
		[HttpGet]
		public ActionResult ExportCashDistributionDetail(FormCollection collection) {
			ExportCashDistributionDetailModel model = new ExportCashDistributionDetailModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				model.CashDistributionReportDetail = ReportRepository.FindCapitalDistribution(model.CapitalDistributionId);
			}
			return View(model);
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

		public JsonResult CapitalCallSummaryList(FormCollection collection) {
			CapitalCallSummaryModel model = new CapitalCallSummaryModel();
			this.TryUpdateModel(model, collection);
			string error = string.Empty;
			ResultModel resultModel = new ResultModel();
			CapitalCallReportDetail detail = null;
			if (ModelState.IsValid) {
				detail = ReportRepository.FindCapitalCall(model.CapitalCallId);
			}
			else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							error += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return Json(new { Error = error, Data = detail }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetCapitalCalls(int fundId) {
			List<Models.Entity.CapitalCall> capitalCalls = CapitalCallRepository.GetCapitalCalls(fundId);
			List<SelectListItem> selectLists = new List<SelectListItem>();
			foreach (var capitalCall in capitalCalls) {
				selectLists.Add(new SelectListItem { Selected = false, Text = capitalCall.CapitalCallNumber + "# (" + capitalCall.CapitalCallDueDate.ToString("MM/dd/yyyy") + ")", Value = capitalCall.CapitalCallID.ToString() });
			}
			return Json(selectLists, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/ExportCapitalCallDetail
		[HttpGet]
		public ActionResult ExportCapitalCallDetail(FormCollection collection) {
			ExportCapitalCallDetailModel model = new ExportCapitalCallDetailModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				model.CapitalCallReportDetail = ReportRepository.FindCapitalCall(model.CapitalCallId);
			}
			return View(model);
		}

		#endregion

		#region DealDetail

		public ActionResult DealDetail() {
			ViewData["MenuName"] = "Reports";
			ViewData["PageName"] = "DealDetail";
			DealDetailModel model = new DealDetailModel();
			return View(model);
		}

		[HttpPost]
		public JsonResult DealDetailReport(FormCollection collection) {
			DealDetailModel model = new DealDetailModel();
			this.TryUpdateModel(model, collection);
			string error = string.Empty;
			ResultModel resultModel = new ResultModel();
			DealDetailReportModel dealReportDetail = null;
			if (ModelState.IsValid) {
				dealReportDetail = ReportRepository.FindDealDetailReport(model.DealId);
			}
			else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							error += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return Json(new { Error = error, Data = dealReportDetail }, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/ExportDetail
		[HttpGet]
		public ActionResult ExportDealDetail(FormCollection collection) {
			ExportDealDetailModel model = new ExportDealDetailModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				model.DealDetailReportModel = ReportRepository.FindDealDetailReport(model.DealId);
			}
			return View(model);
		}

		#endregion

		#region DealOrigination

		public ActionResult DealOrigination() {
			ViewData["MenuName"] = "Reports";
			ViewData["PageName"] = "DealOrigination";
			DealOriginationModel model = new DealOriginationModel();
			return View(model);
		}

		//
		// POST: /Deal/DealOriginationReport
		[HttpPost]
		public JsonResult DealOriginationReport(FormCollection collection) {
			DealOriginationModel model = new DealOriginationModel();
			this.TryUpdateModel(model, collection);
			string error = string.Empty;
			ResultModel resultModel = new ResultModel();
			DealOriginationReportModel dealOriginationReportDetail = null;
			if (ModelState.IsValid) {
				dealOriginationReportDetail = ReportRepository.FindDealOriginationReport(model.DealId);
			}
			else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							error += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return Json(new { Error = error, Data = dealOriginationReportDetail }, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/ExportDealOriginationDetail
		[HttpGet]
		public ActionResult ExportDealOriginationDetail(FormCollection collection) {
			ExportDealOriginationModel model = new ExportDealOriginationModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				model.DealOriginationReportModel = ReportRepository.FindDealOriginationReport(model.DealId);
			}
			return View(model);
		}

		#endregion

		#region FundBreakDown

		public ActionResult FundBreakDown() {
			ViewData["MenuName"] = "Reports";
			ViewData["PageName"] = "FundBreakDown";
			FundBreakDownModel model = new FundBreakDownModel();
			return View(model);
		}

		//
		// POST: /Deal/FundBreakDownReport
		[HttpPost]
		public JsonResult FundBreakDownReport(FormCollection collection) {
			FundBreakDownModel model = new FundBreakDownModel();
			this.TryUpdateModel(model, collection);
			string error = string.Empty;
			ResultModel resultModel = new ResultModel();
			FundBreakDownReportDetail fundBreakDownReportDetail = null;
			if (ModelState.IsValid) {
				fundBreakDownReportDetail = ReportRepository.FindFundBreakDownReport(model.FundId);
			}
			else {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							error += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return Json(new { Error = error, Data = fundBreakDownReportDetail }, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Deal/ExportFundBreakDownDetail
		[HttpGet]
		public ActionResult ExportFundBreakDownDetail(FormCollection collection) {
			ExportFundBreakDownDetailModel model = new ExportFundBreakDownDetailModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				model.FundBreakDownReportDetail = ReportRepository.FindFundBreakDownReport(model.FundId);
			}
			return View(model);
		}

		#endregion
	}
}
