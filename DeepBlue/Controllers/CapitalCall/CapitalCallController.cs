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

		#region New Capital Call
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
			CreateReqularModel model = new CreateReqularModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				Models.Entity.CapitalCall capitalCall = new Models.Entity.CapitalCall();
				CapitalCallLineItem item;
				capitalCall.CapitalAmountCalled = model.CapitalAmountCalled;
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
					decimal totalCommittedAmount = FundRepository.FindTotalCommittedAmount(capitalCall.FundID, (int)Models.Investor.Enums.InvestorType.NonManagingMember);
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
		#endregion

		#region Manual Capital Call
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
			CreateManualModel model = new CreateManualModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				Models.Entity.CapitalCall capitalCall = new Models.Entity.CapitalCall();
				CapitalCallLineItem item;
				capitalCall.CapitalAmountCalled = model.CapitalAmountCalled;
				capitalCall.CapitalCallDate = model.CapitalCallDate;
				capitalCall.CapitalCallDueDate = model.CapitalCallDueDate;
				capitalCall.CapitalCallNumber = string.Empty;
				capitalCall.CapitalCallTypeID = (int)Models.CapitalCall.Enums.CapitalCallType.Manual;
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
		#endregion

		#region Receive Capital Call
		//
		// GET: /CapitalCall/Receive
		[HttpGet]
		public ActionResult Receive(int? id, int? fundId) {
			ViewData["MenuName"] = "Fund Tracker";
			CreateReceiveModel model = new CreateReceiveModel();
			model.CapitalCallId = id ?? 0;
			model.FundId = fundId ?? 0;
			model.CapitalCalls = new List<SelectListItem>();
			model.CapitalCalls.Add(new SelectListItem {
				Value = "0", Text = "--Select One--", Selected = false
			});
			model.Items = new List<CapitalCallLineItemDetail>();
			model.Items.Add(new CapitalCallLineItemDetail());
			return View(model);
		}

		//
		// POST: /CapitalCall/CreateReceiveCapitalCall
		[HttpPost]
		public ActionResult CreateReceiveCapitalCall(FormCollection collection) {
			CreateReceiveModel model = new CreateReceiveModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				Models.Entity.CapitalCall capitalCall = CapitalCallRepository.FindCapitalCall(model.CapitalCallId);
				CapitalCallLineItem item;
				capitalCall.CapitalAmountCalled = model.CapitalAmountCalled;
				capitalCall.CapitalCallDate = model.CapitalCallDate;
				capitalCall.CapitalCallDueDate = model.CapitalCallDueDate;
				capitalCall.LastUpdatedBy = AppSettings.CreatedByUserId;
				capitalCall.LastUpdatedDate = DateTime.Now;
				int index;
				for (index = 1; index < model.ItemCount + 1; index++) {
					item = capitalCall.CapitalCallLineItems.SingleOrDefault(capitalCallItem => capitalCallItem.CapitalCallLineItemID == Convert.ToInt32(collection[index.ToString() + "_" + "CapitalCallLineItemId"]));
					if (item != null) {
						item.LastUpdatedBy = AppSettings.CreatedByUserId;
						item.LastUpdatedDate = DateTime.Now;
						if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "CapitalAmountCalled"]) == false) {
							item.CapitalAmountCalled = Convert.ToDecimal(collection[index.ToString() + "_" + "CapitalAmountCalled"]);
						}
						if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ManagementFees"]) == false) {
							item.ManagementFees = Convert.ToDecimal(collection[index.ToString() + "_" + "ManagementFees"]);
						}
						if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "InvestmentAmount"]) == false) {
							item.InvestmentAmount = Convert.ToDecimal(collection[index.ToString() + "_" + "InvestmentAmount"]);
						}
						if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "InvestedAmountInterest"]) == false) {
							item.InvestedAmountInterest = Convert.ToDecimal(collection[index.ToString() + "_" + "InvestedAmountInterest"]);
						}
						if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ManagementFeeInterest"]) == false) {
							item.ManagementFeeInterest = Convert.ToDecimal(collection[index.ToString() + "_" + "ManagementFeeInterest"]);
						}
						if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "Received"]) == false) {
							if (collection[index.ToString() + "_" + "Received"].Contains("true")) {
								if (string.IsNullOrEmpty(collection[index.ToString() + "_" + "ReceivedDate"]) == false) {
									item.ReceivedDate = Convert.ToDateTime(collection[index.ToString() + "_" + "ReceivedDate"]);
								}
							}
						}
						if ((item.ReceivedDate ?? Convert.ToDateTime("01/01/1900")).Year <= 1900) {
							item.ReceivedDate = null;
						}
					}
				}
				IEnumerable<ErrorInfo> errorInfo = CapitalCallRepository.SaveCapitalCall(capitalCall);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
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
			return View("Result",resultModel);
		}

		#endregion

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

		#region Distribution
		public ActionResult NewCapitalDistribution() {
			ViewData["MenuName"] = "Fund Tracker";
			return View();
		}

		public ActionResult CreateDistribution(FormCollection collection) {
			CreateDistributionModel model = new CreateDistributionModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				CapitalDistribution distribution = new CapitalDistribution();
				CapitalDistributionLineItem item;
				distribution.CapitalDistributionDate = model.DistributionDate;
				distribution.CapitalDistributionDueDate = model.DistributionDueDate;
				if (model.AddPreferredReturn)
					distribution.PreferredReturn = model.PreferredReturn;
				if (model.AddReturnManagementFees)
					distribution.ReturnManagementFees = model.ReturnManagementFees;
				if (model.AddReturnFundExpenses)
					distribution.ReturnFundExpenses = model.ReturnFundExpenses;
				if (model.AddPreferredCatchUp)
					distribution.PreferredCatchUp = model.PreferredCatchUp;
				if (model.AddProfits) {
					distribution.Profits = model.GPProfits;
					distribution.LPProfits = model.LPProfits;
				}
				distribution.CreatedBy = AppSettings.CreatedByUserId;
				distribution.CreatedDate = DateTime.Now;
				distribution.DistributionAmount = model.DistributionAmount;
				distribution.DistributionNumber = model.DistributionNumber;
				distribution.FundID = model.FundId;
				distribution.IsManual = false;
				distribution.LastUpdatedBy = AppSettings.CreatedByUserId;
				distribution.LastUpdatedDate = DateTime.Now;
				List<InvestorFund> investorFunds = CapitalCallRepository.GetAllInvestorFunds(distribution.FundID);
				decimal nonManagingMemberTotalCommitment = investorFunds.Where(fund => fund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.NonManagingMember).Sum(fund => fund.TotalCommitment);
				decimal managingMemberTotalCommitment = investorFunds.Where(fund => fund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.ManagingMember).Sum(fund => fund.TotalCommitment);
				decimal totalCommitment = nonManagingMemberTotalCommitment + managingMemberTotalCommitment;
				foreach (var investorFund in investorFunds) {
					item = new CapitalDistributionLineItem();
					item.CapitalReturn = 0;
					item.CreatedBy = AppSettings.CreatedByUserId;
					item.CreatedDate = DateTime.Now;
					item.DistributionAmount = model.DistributionAmount;
					item.InvestorID = investorFund.InvestorID;
					item.LastUpdatedBy = AppSettings.CreatedByUserId;
					item.LastUpdatedDate = DateTime.Now;

					if (distribution.PreferredCatchUp > 0) {
						// ManagingMember investor type only
						if (investorFund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.ManagingMember) {
							item.PreferredCatchUp = distribution.PreferredCatchUp * (investorFund.TotalCommitment / managingMemberTotalCommitment);
						}
					} else {
						item.PreferredCatchUp = 0;
					}
					if (distribution.PreferredReturn > 0) {
						// NonManagingMember investor type only
						if (investorFund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.NonManagingMember) {
							item.PreferredReturn = distribution.PreferredReturn * (investorFund.TotalCommitment / nonManagingMemberTotalCommitment);
						}
					} else {
						item.PreferredReturn = 0;
					}
					if (distribution.ReturnFundExpenses > 0) {
						// NonManagingMember investor type only
						if (investorFund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.NonManagingMember) {
							item.ReturnFundExpenses = distribution.ReturnFundExpenses * (investorFund.TotalCommitment / nonManagingMemberTotalCommitment);
						}
					} else {
						item.ReturnFundExpenses = 0;
					}
					if (distribution.ReturnManagementFees > 0) {
						// NonManagingMember investor type only
						if (investorFund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.NonManagingMember) {
							item.ReturnManagementFees = distribution.ReturnManagementFees * (investorFund.TotalCommitment / nonManagingMemberTotalCommitment);
						}
					} else {
						item.ReturnManagementFees = 0;
					}
					if (distribution.Profits > 0) {
						// ManagingMember investor type only
						if (investorFund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.ManagingMember) {
							item.Profits = distribution.Profits * (investorFund.TotalCommitment / managingMemberTotalCommitment);
						}
					}
					if (distribution.LPProfits > 0) {
						// NonManagingMember investor type only
						if (investorFund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.NonManagingMember) {
							item.Profits = distribution.LPProfits * (investorFund.TotalCommitment / nonManagingMemberTotalCommitment);
						}
					}
					distribution.CapitalDistributionLineItems.Add(item);
				}
				IEnumerable<ErrorInfo> errorInfo = CapitalCallRepository.SaveCapitalDistribution(distribution);
				if (errorInfo != null) {
					foreach (var err in errorInfo.ToList()) {
						resultModel.Result += err.PropertyName + " : " + err.ErrorMessage + "\n";
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

		public ActionResult CapitalDistributionList(int? id) {
			ViewData["MenuName"] = "Fund Tracker";
			CapitalDistributionListModel model = new CapitalDistributionListModel();
			model.FundId = id ?? 0;
			if (model.FundId > 0) {
				Models.Entity.Fund fund = CapitalCallRepository.FindFund(model.FundId);
				model.FundName = fund.FundName;
			}
			return View(model);
		}

		//
		// GET: /CapitalCall/CapitalCallList
		[HttpGet]
		public ActionResult CapitalDistributionDetailList(int pageIndex, int pageSize, string sortName, string sortOrder, int fundId) {
			int totalRows = 0;
			List<Models.Entity.CapitalDistribution> capitalCalls = CapitalCallRepository.GetCapitalDistributions(pageIndex, pageSize, sortName, sortOrder, ref totalRows, fundId);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(capitalCalls);
		}
		#endregion

		//
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
				detail.DistributionNumber = (fund.CapitalDistributions.Count + 1).ToString();
			}
			return Json(detail, JsonRequestBehavior.AllowGet);
		}

		//GET : /CapitalCall/CapitalCallDetail
		public JsonResult CapitalCallDetail(int id) {
			return Json(CapitalCallRepository.FindCapitalCallDetail(id), JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /CapitalCall/CapitalCallList
		[HttpGet]
		public ActionResult CapitalCallList(int pageIndex, int pageSize, string sortName, string sortOrder, int fundId) {
			int totalRows = 0;
			List<Models.Entity.CapitalCall> capitalCalls = CapitalCallRepository.GetCapitalCalls(pageIndex, pageSize, sortName, sortOrder, ref totalRows, fundId);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(capitalCalls);
		}

		//
		// GET: /CapitalCall/CapitalCallList
		[HttpGet]
		public JsonResult GetCapitalCallList(int fundId) {
			List<Models.Entity.CapitalCall> capitalCalls = CapitalCallRepository.GetCapitalCalls(fundId);
			List<SelectListItem> selectLists = new List<SelectListItem>();
			foreach (var capitalCall in capitalCalls) {
				selectLists.Add(new SelectListItem { Selected = false, Text = capitalCall.CapitalCallNumber + "# (" + capitalCall.CapitalCallDate.ToString("MM/dd/yyyy") + ")", Value = capitalCall.CapitalCallID.ToString() });
			}
			return Json(selectLists, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /CapitalCall/FindCapitalCall
		[HttpGet]
		public JsonResult FindCapitalCall(int id) {
			CreateReceiveModel model = new CreateReceiveModel();
			Models.Entity.CapitalCall capitalCall = CapitalCallRepository.FindCapitalCall(id);
			if (capitalCall != null) {
				model.FundId = capitalCall.Fund.FundID;
				model.FundName = capitalCall.Fund.FundName;
				model.CapitalCallNumber = capitalCall.CapitalCallNumber;
				model.CapitalAmountCalled = capitalCall.CapitalAmountCalled;
				model.CapitalCallDate = capitalCall.CapitalCallDate;
				model.CapitalCallDueDate = capitalCall.CapitalCallDueDate;
				model.CapitalCallId = capitalCall.CapitalCallID;
				model.Items = new List<CapitalCallLineItemDetail>();
				foreach (var item in capitalCall.CapitalCallLineItems) {
					model.Items.Add(new CapitalCallLineItemDetail {
						InvestorName = item.Investor.InvestorName,
						CapitalAmountCalled = item.CapitalAmountCalled,
						InvestedAmountInterest = item.InvestedAmountInterest ?? 0,
						CapitalCallLineItemId = item.CapitalCallLineItemID,
						InvestmentAmount = item.InvestedAmountInterest ?? 0,
						ManagementFeeInterest = item.ManagementFeeInterest ?? 0,
						ManagementFees = item.ManagementFees ?? 0,
						Received = (item.ReceivedDate.HasValue ? true : false),
						ReceivedDate = item.ReceivedDate ?? Convert.ToDateTime("01/01/1900")
					});
				}
			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		//
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

		//
		//GET : /CapitalCall/Result
		public ActionResult Result() {
			return View();
		}
	}
}
