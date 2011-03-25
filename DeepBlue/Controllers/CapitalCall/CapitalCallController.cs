using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Controllers.Fund;
using DeepBlue.Models.CapitalCall;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Controllers.CapitalCall {
	public class CapitalCallController : Controller {

		public IFundRepository FundRepository { get; set; }

		public ICapitalCallRepository CapitalCallRepository { get; set; }

		public CapitalCallController()
			: this(new FundRepository(), new CapitalCallRepository()) {
		}

		public CapitalCallController(IFundRepository fundRepository, ICapitalCallRepository adminRepository) {
			FundRepository = fundRepository;
			CapitalCallRepository = adminRepository;
		}


		//
		// GET: /CapitalCall/New
		public ActionResult New() {
			ViewData["MenuName"] = "Fund Tracker";
			return View();
		}

		//
		// POST: /CapitalCall/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			CreateReqularCapitalCallModel model = new CreateReqularCapitalCallModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				Models.Entity.CapitalCall capitalCall = new Models.Entity.CapitalCall();
				CapitalCallLineItem item;
				capitalCall.CapitalAmountCalled = model.CapitalAmount;
				capitalCall.CapitalCallDate = model.CapitalCallDate;
				capitalCall.CapitalCallDueDate = model.CapitalCallDueDate;
				capitalCall.CapitalCallNumber = string.Empty;
				capitalCall.CapitalCallTypeID = (int)Models.CapitalCall.Enums.CapitalCallType.Reqular;
				capitalCall.CreatedBy = AppSettings.CreatedByUserId;
				capitalCall.CreatedDate = DateTime.Now;
				capitalCall.ExistingInvestmentAmount = model.ExistingInvestmentAmount ?? 0;
				capitalCall.LastUpdatedBy = AppSettings.CreatedByUserId;
				capitalCall.LastUpdatedDate = DateTime.Now;
				capitalCall.NewInvestmentAmount = model.NewInvestmentAmount;
				capitalCall.FundID = model.FundId;
				capitalCall.FundExpenses = 0;
				capitalCall.ManagementFees = 0;
				capitalCall.InvestedAmountInterest = 0;
				capitalCall.CapitalCallNumber = model.CapitalCallNumber;
				if (model.AddFundExpenses) {
					capitalCall.FundExpenses = model.FundExpenseAmount ?? 0;
				}
				if (model.AddManagementFees) {
					decimal totalCommittedAmount =  FundRepository.FindTotalCommittedAmount(capitalCall.FundID, (int)Models.Investor.Enums.InvestorType.NonManagingMember);
					capitalCall.ManagementFeeStartDate = model.FromDate;
					capitalCall.ManagementFeeEndDate = model.ToDate;
					capitalCall.ManagementFees = model.ManagementFees ?? 0;
					capitalCall.ManagementFeeInterest = 0;
					List<InvestorFund> investorFunds = CapitalCallRepository.GetAllInvestorFunds(capitalCall.FundID);
					foreach (var investorFund in investorFunds) {
						item = new CapitalCallLineItem();
						item.CapitalAmountCalled = 0;
						item.CreatedBy = AppSettings.CreatedByUserId;
						item.CreatedDate = DateTime.Now;
						item.ExistingInvestmentAmount = 0;
						item.FundExpenses = 0;
						item.InvestedAmountInterest = 0;
						item.InvestmentAmount = 0;
						item.InvestorID = investorFund.InvestorID;
						item.LastUpdatedBy = AppSettings.CreatedByUserId;
						item.LastUpdatedDate = DateTime.Now;
						item.ManagementFeeInterest = 0;
						item.ManagementFees = (investorFund.TotalCommitment / totalCommittedAmount) * capitalCall.ManagementFees;
						item.NewInvestmentAmount = 0;
						capitalCall.CapitalCallLineItems.Add(item);
					}
				}
				if (!((capitalCall.NewInvestmentAmount + capitalCall.ExistingInvestmentAmount) == (capitalCall.CapitalAmountCalled - capitalCall.ManagementFees - capitalCall.FundExpenses))) {
					ModelState.AddModelError("NewInvestmentAmount", "(New Investment Amount + Existing Investment Amount) should be equal to (Capital Amount - Management Fees - Fund Expenses).");
				} else {
					IEnumerable<ErrorInfo> errorInfo = CapitalCallRepository.SaveCapitalCall(capitalCall);
					if (errorInfo != null) {
						foreach (var err in errorInfo.ToList()) {
							resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
						}
					}
				}
			}
			if (ModelState.IsValid == false) {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							resultModel.Result += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return View("Result", resultModel);
		}

		//
		// GET: /CapitalCall/New
		public ActionResult NewManualCapitalCall() {
			ViewData["MenuName"] = "Fund Tracker";
			return View();
		}
		//
		// POST: /CapitalCall/CreateManualCapitalCall
		[HttpPost]
		public ActionResult CreateManualCapitalCall(FormCollection collection) {
			CreateManualCapitalCallModel model = new CreateManualCapitalCallModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				Models.Entity.CapitalCall capitalCall = new Models.Entity.CapitalCall();
				CapitalCallLineItem item;
				capitalCall.CapitalAmountCalled = model.CapitalCallAmount;
				capitalCall.CapitalCallDate = model.CapitalCallDate;
				capitalCall.CapitalCallDueDate = model.CapitalCallDueDate;
				capitalCall.CapitalCallNumber = string.Empty;
				capitalCall.CapitalCallTypeID = (int)Models.CapitalCall.Enums.CapitalCallType.Reqular;
				capitalCall.CreatedBy = AppSettings.CreatedByUserId;
				capitalCall.CreatedDate = DateTime.Now;
				capitalCall.ExistingInvestmentAmount = model.ExistingInvestmentAmount ?? 0;
				capitalCall.LastUpdatedBy = AppSettings.CreatedByUserId;
				capitalCall.LastUpdatedDate = DateTime.Now;
				capitalCall.NewInvestmentAmount = model.NewInvestmentAmount ?? 0;
				capitalCall.FundID = model.FundId;
				capitalCall.InvestedAmountInterest = model.InvestedAmount ?? 0;
				capitalCall.InvestedAmountInterest = model.InvestedAmountInterest ?? 0;
				capitalCall.FundExpenses = model.FundExpenses ?? 0;
				capitalCall.ManagementFees = model.ManagementFees ?? 0;
				capitalCall.ManagementFeeInterest = model.ManagementFeeInterest ?? 0;
				capitalCall.CapitalCallNumber = model.CapitalCallNumber;
				int index;
				for (index = 1; index < model.InvestorCount + 1; index++) {
					item = new CapitalCallLineItem();
					item.CreatedBy = AppSettings.CreatedByUserId;
					item.CreatedDate = DateTime.Now;
					item.LastUpdatedBy = AppSettings.CreatedByUserId;
					item.LastUpdatedDate = DateTime.Now;
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "CapitalCallAmount"]) == false) {
						item.CapitalAmountCalled = Convert.ToDecimal(collection[index.ToString() + "_" + "CapitalCallAmount"]);
					}
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ManagementFeeInterest"]) == false) {
						item.ManagementFeeInterest = Convert.ToDecimal(collection[index.ToString() + "_" + "ManagementFeeInterest"]);
					}
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "InvestedAmountInterest"]) == false) {
						item.InvestedAmountInterest = Convert.ToDecimal(collection[index.ToString() + "_" + "InvestedAmountInterest"]);
					}
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ManagementFees"]) == false) {
						item.ManagementFees = Convert.ToDecimal(collection[index.ToString() + "_" + "ManagementFees"]);
					}
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "FundExpenses"]) == false) {
						item.FundExpenses = Convert.ToDecimal(collection[index.ToString() + "_" + "FundExpenses"]);
					}
					if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "InvestorId"]) == false) {
						item.InvestorID = Convert.ToInt32(collection[index.ToString() + "_" + "InvestorId"]);
					}
					if (item.InvestorID > 0) {
						capitalCall.CapitalCallLineItems.Add(item);
					}
				}
				if (capitalCall.CapitalCallLineItems.Count == 0) {
					ModelState.AddModelError("InvestorCount", "Select any one investor");
				} else {
					IEnumerable<ErrorInfo> errorInfo = CapitalCallRepository.SaveCapitalCall(capitalCall);
					if (errorInfo != null) {
						foreach (var err in errorInfo.ToList()) {
							resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
						}
					}
				}
			}
			if (ModelState.IsValid == false) {
				foreach (var values in ModelState.Values.ToList()) {
					foreach (var err in values.Errors.ToList()) {
						if (string.IsNullOrEmpty(err.ErrorMessage) == false) {
							resultModel.Result += err.ErrorMessage + "\n";
						}
					}
				}
			}
			return View("Result", resultModel);
		}


		//GET : /CapitalCall/FundDetail
		[HttpGet]
		public JsonResult FundDetail(int id) {
			Models.Entity.Fund fund = CapitalCallRepository.FindFund(id);
			FundDetail detail = new FundDetail();
			if (fund != null) {
				detail.FundName = fund.FundName;
				detail.FundId = fund.FundID;
				detail.TotalCommitment = string.Format("{0:C}", fund.InvestorFunds.Sum(investorFund => investorFund.TotalCommitment));
				detail.UnfundedAmount = string.Format("{0:C}", fund.InvestorFunds.Sum(investorFund => investorFund.UnfundedAmount ?? 0));
				detail.CapitalCallNumber = (fund.CapitalCalls.Count + 1).ToString();
			}
			return Json(detail, JsonRequestBehavior.AllowGet);
		}

		//GET : /CapitalCall/CapitalCallDetail
		public JsonResult CapitalCallDetail(int id) {
			return Json(CapitalCallRepository.FindCapitalCallDetail(id), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Admin/CapitalCallList
		[HttpGet]
		public ActionResult CapitalCallList(int pageIndex, int pageSize, string sortName, string sortOrder, int fundId) {
			int totalRows = 0;
			List<Models.Entity.CapitalCall> capitalCalls = CapitalCallRepository.GetCapitalCalls(pageIndex, pageSize, sortName, sortOrder, ref totalRows, fundId);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(capitalCalls);
		}


		#region Manament Fee Calculation
		//GET : /CapitalCall/CalculateManagementFee
		[HttpGet]
		public JsonResult CalculateManagementFee(int fundId, DateTime startDate, DateTime endDate) {
			ManagementFeeDetail detail = new ManagementFeeDetail();
			detail.Tiers = new FlexigridData();

			endDate = startDate.AddMonths(3);
			Models.Entity.Fund fund = CapitalCallRepository.FindFund(fundId);
			if (fund != null) {
				decimal totalCommittedAmount = fund.InvestorFunds.Where(investorFund => investorFund.InvestorTypeId == (int)Models.Investor.Enums.InvestorType.NonManagingMember).Sum(invfund => invfund.TotalCommitment);
				FundRateSchedule fundRateSchedule = fund.FundRateSchedules.FirstOrDefault(schedule => schedule.InvestorTypeID == (int)Models.Investor.Enums.InvestorType.NonManagingMember);
				if (totalCommittedAmount > 0 && fundRateSchedule != null) {
					ManagementFeeRateSchedule manFeeRateSchedule = FundRepository.FindManagementFeeRateSchedule(fundRateSchedule.RateScheduleID);
					if (manFeeRateSchedule != null) {
						List<ManagementFeeRateScheduleTier> tiers = manFeeRateSchedule.ManagementFeeRateScheduleTiers.Where(feeTier => feeTier.StartDate <= startDate && feeTier.EndDate >= startDate || feeTier.StartDate <= endDate && feeTier.EndDate >= endDate).ToList();
						FlexigridRow tierDetail;
						switch (tiers.Count) {
							case 1:
								tierDetail = new FlexigridRow();
								tierDetail.cell.Add(tiers[0].StartDate.ToString("MM/dd/yyyy"));
								tierDetail.cell.Add(tiers[0].EndDate.ToString("MM/dd/yyyy"));
								if (tiers[0].MultiplierTypeID == (int)Models.Fund.Enums.MutiplierType.CapitalCommitted) {
									detail.ManagementFee = CalculateCapitalFee(totalCommittedAmount, tiers[0].Multiplier, 90);
									tierDetail.cell.Add("CapitalCommitted");
									tierDetail.cell.Add(tiers[0].Multiplier.ToString("0.00"));
									tierDetail.cell.Add(string.Empty);
								} else {
									detail.ManagementFee = CalculateFlatFee(totalCommittedAmount, tiers[0].Multiplier, 90);
									tierDetail.cell.Add("FlatFee");
									tierDetail.cell.Add(string.Empty);
									tierDetail.cell.Add(tiers[0].Multiplier.ToString("0.00"));
								}
								detail.Tiers.rows.Add(tierDetail);
								break;
							case 2:
								tierDetail = new FlexigridRow();
								tierDetail.cell.Add(tiers[0].StartDate.ToString("MM/dd/yyyy"));
								tierDetail.cell.Add(tiers[0].EndDate.ToString("MM/dd/yyyy"));
								if (tiers[0].MultiplierTypeID == (int)Models.Fund.Enums.MutiplierType.CapitalCommitted) {
									detail.ManagementFee += CalculateCapitalFee(totalCommittedAmount, tiers[0].Multiplier, (tiers[0].EndDate - startDate).TotalDays);
									tierDetail.cell.Add("CapitalCommitted");
									tierDetail.cell.Add(tiers[0].Multiplier.ToString("0.00"));
									tierDetail.cell.Add(string.Empty);
								} else {
									detail.ManagementFee += CalculateFlatFee(totalCommittedAmount, tiers[0].Multiplier, (tiers[0].EndDate - startDate).TotalDays);
									tierDetail.cell.Add("FlatFee");
									tierDetail.cell.Add(string.Empty);
									tierDetail.cell.Add(tiers[0].Multiplier.ToString("0.00"));
								}
								detail.Tiers.rows.Add(tierDetail);
								tierDetail = new FlexigridRow();
								tierDetail.cell.Add(tiers[1].StartDate.ToString("MM/dd/yyyy"));
								tierDetail.cell.Add(tiers[1].EndDate.ToString("MM/dd/yyyy"));
								if (tiers[1].MultiplierTypeID == (int)Models.Fund.Enums.MutiplierType.CapitalCommitted) {
									detail.ManagementFee += CalculateCapitalFee(totalCommittedAmount, tiers[1].Multiplier, (90 - (tiers[0].EndDate - startDate).TotalDays));
									tierDetail.cell.Add("CapitalCommitted");
									tierDetail.cell.Add(tiers[1].Multiplier.ToString("0.00"));
									tierDetail.cell.Add(string.Empty);
								} else {
									detail.ManagementFee += CalculateFlatFee(totalCommittedAmount, tiers[1].Multiplier, (90 - (tiers[0].EndDate - startDate).TotalDays));
									tierDetail.cell.Add("FlatFee");
									tierDetail.cell.Add(string.Empty);
									tierDetail.cell.Add(tiers[1].Multiplier.ToString("0.00"));
								}
								detail.Tiers.rows.Add(tierDetail);
								break;
						}
					}
				}
			}
			detail.Tiers.page = 1;
			detail.Tiers.total = detail.Tiers.rows.Count;
			return Json(detail, JsonRequestBehavior.AllowGet);
		}

		private decimal CalculateCapitalFee(decimal totalCommittedAmount, decimal multiplier, double days) {
			return (totalCommittedAmount * (multiplier / 100) * decimal.Divide((decimal)days, (decimal)360));
		}

		private decimal CalculateFlatFee(decimal totalCommittedAmount, decimal multiplier, double days) {
			return (totalCommittedAmount * multiplier * decimal.Divide((decimal)days, (decimal)360));
		}

		#endregion

		//GET : /CapitalCall/List
		public ActionResult List(int? id) {
			ViewData["MenuName"] = "Fund Tracker";
			ListModel model = new ListModel();
			model.FundId = id ?? 0;
			if (model.FundId > 0) {
				Models.Entity.Fund fund = CapitalCallRepository.FindFund(model.FundId);
				model.FundName = fund.FundName;
			}
			return View(model);
		}

		public ActionResult Result() {
			return View();
		}
	}
}
