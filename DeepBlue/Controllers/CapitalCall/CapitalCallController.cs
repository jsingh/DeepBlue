using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Controllers.Fund;
using DeepBlue.Models.CapitalCall;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Investor;
using System.Text;
using DeepBlue.Models.CapitalCall.Enums;

namespace DeepBlue.Controllers.CapitalCall {
	public class CapitalCallController : Controller {

		public IFundRepository FundRepository { get; set; }

		public ICapitalCallRepository CapitalCallRepository { get; set; }

		public IInvestorRepository InvestorRepository { get; set; }

		public CapitalCallController()
			: this(new FundRepository(), new CapitalCallRepository(), new InvestorRepository()) {
		}

		public CapitalCallController(IFundRepository fundRepository, ICapitalCallRepository adminRepository, IInvestorRepository investorRepository) {
			FundRepository = fundRepository;
			CapitalCallRepository = adminRepository;
			InvestorRepository = investorRepository;
		}

		#region New Capital Call

		//
		// GET: /CapitalCall/New
		public ActionResult New() {
			ViewData["MenuName"] = "Fund Tracker";
			ViewData["SubmenuName"] = "CapitalCall";
			ViewData["PageName"] = "CapitalCall";
			return View();
		}

		//
		// POST: /CapitalCall/Create
		[HttpPost]
		public ActionResult Create(FormCollection collection) {
			CreateCapitalCallModel model = new CreateCapitalCallModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model, collection);
			if ((model.NewInvestmentAmount ?? 0) <= 0) {
				ModelState.AddModelError("NewInvestmentAmount", "New Investment Amount is required");
			}
			if (ModelState.IsValid) {

				// Attempt to create capital call.

				Models.Entity.CapitalCall capitalCall = new Models.Entity.CapitalCall();
				CapitalCallLineItem item;

				capitalCall.CapitalAmountCalled = model.CapitalAmountCalled;
				capitalCall.CapitalCallDate = model.CapitalCallDate;
				capitalCall.CapitalCallDueDate = model.CapitalCallDueDate;
				capitalCall.CapitalCallNumber = string.Empty;
				capitalCall.CapitalCallTypeID = (int)Models.CapitalCall.Enums.CapitalCallType.Reqular;
				capitalCall.CreatedBy = AppSettings.CreatedByUserId;
				capitalCall.CreatedDate = DateTime.Now;
				capitalCall.LastUpdatedBy = AppSettings.CreatedByUserId;
				capitalCall.LastUpdatedDate = DateTime.Now;
				capitalCall.ExistingInvestmentAmount = model.ExistingInvestmentAmount ?? 0;
				capitalCall.NewInvestmentAmount = model.NewInvestmentAmount;
				capitalCall.FundID = model.FundId;
				capitalCall.FundExpenses = 0;
				capitalCall.ManagementFees = 0;
				capitalCall.CapitalCallNumber = Convert.ToString(CapitalCallRepository.FindCapitalCallNumber(model.FundId));
				capitalCall.InvestmentAmount = (capitalCall.NewInvestmentAmount ?? 0) + (capitalCall.ExistingInvestmentAmount ?? 0);
				capitalCall.InvestedAmountInterest = 0;
				if ((model.AddFundExpenses ?? false)) {
					capitalCall.FundExpenses = model.FundExpenseAmount ?? 0;
				}
				List<InvestorFund> investorFunds = CapitalCallRepository.GetAllInvestorFunds(capitalCall.FundID);
				if (investorFunds != null) {

					// Find non managing total commitment.
					decimal nonManagingMemberTotalCommitment = investorFunds.Where(fund => fund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.NonManagingMember).Sum(fund => fund.TotalCommitment);
					// Find managing total commitment.
					decimal managingMemberTotalCommitment = investorFunds.Where(fund => fund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.ManagingMember).Sum(fund => fund.TotalCommitment);
					// Calculate managing total commitment.
					decimal totalCommitment = nonManagingMemberTotalCommitment + managingMemberTotalCommitment;

					if ((model.AddManagementFees ?? false)) {
						capitalCall.ManagementFeeStartDate = model.FromDate;
						capitalCall.ManagementFeeEndDate = model.ToDate;
						capitalCall.ManagementFees = model.ManagementFees ?? 0;
						capitalCall.ManagementFeeInterest = 0;
					}

					// Check new investment amount and existing investment amount.
					if (!((capitalCall.NewInvestmentAmount + capitalCall.ExistingInvestmentAmount) == (capitalCall.CapitalAmountCalled - capitalCall.ManagementFees - capitalCall.FundExpenses))) {
						ModelState.AddModelError("NewInvestmentAmount", "(New Investment Amount + Existing Investment Amount) should be equal to (Capital Amount - Management Fees - Fund Expenses).");
					}
					else {
						foreach (var investorFund in investorFunds) {

							// Attempt to create capital call line item for each investor fund.

							item = new CapitalCallLineItem();
							item.CreatedBy = AppSettings.CreatedByUserId;
							item.CreatedDate = DateTime.Now;
							item.LastUpdatedBy = AppSettings.CreatedByUserId;
							item.LastUpdatedDate = DateTime.Now;
							item.ExistingInvestmentAmount = (investorFund.TotalCommitment / totalCommitment) * capitalCall.ExistingInvestmentAmount;
							item.FundExpenses = (investorFund.TotalCommitment / totalCommitment) * capitalCall.FundExpenses;
							item.InvestedAmountInterest = (investorFund.TotalCommitment / totalCommitment) * capitalCall.InvestedAmountInterest;
							item.InvestmentAmount = (investorFund.TotalCommitment / totalCommitment) * capitalCall.InvestmentAmount;
							item.InvestorID = investorFund.InvestorID;
							item.ManagementFeeInterest = (investorFund.TotalCommitment / nonManagingMemberTotalCommitment) * capitalCall.ManagementFeeInterest;
							item.ManagementFees = (investorFund.TotalCommitment / nonManagingMemberTotalCommitment) * capitalCall.ManagementFees;
							item.NewInvestmentAmount = (investorFund.TotalCommitment / totalCommitment) * capitalCall.NewInvestmentAmount;

							// Calculate capital call amount for each investor.
							item.CapitalAmountCalled = (investorFund.TotalCommitment / totalCommitment) * capitalCall.CapitalAmountCalled;

							// Reduce investor unfunded amount = investor unfunded amount – capital call amount for investor.
							investorFund.UnfundedAmount = investorFund.UnfundedAmount - item.CapitalAmountCalled;

							capitalCall.CapitalCallLineItems.Add(item);
						}
						IEnumerable<ErrorInfo> errorInfo = CapitalCallRepository.SaveCapitalCall(capitalCall);
						if (errorInfo != null) {
							resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
						}
						else {
							foreach (var investorFund in investorFunds) {
								errorInfo = InvestorRepository.SaveInvestorFund(investorFund);
								resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
							}
						}
						if (string.IsNullOrEmpty(resultModel.Result)) {
							resultModel.Result += "True||" + (CapitalCallRepository.FindCapitalCallNumber(model.FundId));
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
		// POST: /CapitalCall/CreateManualCapitalCall
		[HttpPost]
		public ActionResult CreateManualCapitalCall(FormCollection collection) {
			CreateCapitalCallModel model = new CreateCapitalCallModel();
			ResultModel resultModel = new ResultModel();
			List<InvestorFund> investorFunds = new List<InvestorFund>();
			this.TryUpdateModel(model, collection);
			if (ModelState.IsValid) {

				// Attempt to create manual capital call.

				Models.Entity.CapitalCall capitalCall = new Models.Entity.CapitalCall();
				CapitalCallLineItem item;

				capitalCall.CapitalAmountCalled = model.CapitalAmountCalled;
				capitalCall.CapitalCallDate = model.CapitalCallDate;
				capitalCall.CapitalCallDueDate = model.CapitalCallDueDate;
				capitalCall.CapitalCallTypeID = (int)Models.CapitalCall.Enums.CapitalCallType.Manual;
				capitalCall.CreatedBy = AppSettings.CreatedByUserId;
				capitalCall.CreatedDate = DateTime.Now;
				capitalCall.LastUpdatedBy = AppSettings.CreatedByUserId;
				capitalCall.LastUpdatedDate = DateTime.Now;
				capitalCall.ExistingInvestmentAmount = model.ExistingInvestmentAmount ?? 0;
				capitalCall.NewInvestmentAmount = model.NewInvestmentAmount;
				capitalCall.FundID = model.FundId;
				capitalCall.InvestmentAmount = model.InvestedAmount ?? 0;
				capitalCall.InvestedAmountInterest = model.InvestedAmountInterest ?? 0;
				capitalCall.FundExpenses = model.FundExpenses ?? 0;
				capitalCall.ManagementFees = model.ManagementFees ?? 0;
				capitalCall.ManagementFeeInterest = model.ManagementFeeInterest ?? 0;
				capitalCall.CapitalCallNumber = Convert.ToString(CapitalCallRepository.FindCapitalCallNumber(model.FundId));
				int index;
				for (index = 1; index < model.InvestorCount + 1; index++) {
					item = new CapitalCallLineItem();
					item.CreatedBy = AppSettings.CreatedByUserId;
					item.CreatedDate = DateTime.Now;
					item.LastUpdatedBy = AppSettings.CreatedByUserId;
					item.LastUpdatedDate = DateTime.Now;
					item.CapitalAmountCalled = DataTypeHelper.ToDecimal(collection[index.ToString() + "_" + "CapitalAmountCalled"]);
					item.ManagementFeeInterest = DataTypeHelper.ToDecimal(collection[index.ToString() + "_" + "ManagementFeeInterest"]);
					item.InvestedAmountInterest = DataTypeHelper.ToDecimal(collection[index.ToString() + "_" + "InvestedAmountInterest"]);
					item.ManagementFees = DataTypeHelper.ToDecimal(collection[index.ToString() + "_" + "ManagementFees"]);
					item.FundExpenses = DataTypeHelper.ToDecimal(collection[index.ToString() + "_" + "FundExpenses"]);
					item.InvestorID = DataTypeHelper.ToInt32(collection[index.ToString() + "_" + "InvestorId"]);
					if (item.InvestorID > 0) {
						InvestorFund investorFund = InvestorRepository.FindInvestorFund(item.InvestorID, capitalCall.FundID);
						if (investorFund != null) {
							// Reduce investor unfunded amount = investor unfunded amount – capital call amount for investor.
							investorFund.UnfundedAmount = investorFund.UnfundedAmount - item.CapitalAmountCalled;
							investorFunds.Add(investorFund);
						}
						capitalCall.CapitalCallLineItems.Add(item);
					}
				}
				if (capitalCall.CapitalCallLineItems.Count == 0) {
					ModelState.AddModelError("InvestorCount", "Select any one investor");
				}
				else {
					IEnumerable<ErrorInfo> errorInfo = CapitalCallRepository.SaveCapitalCall(capitalCall);
					if (errorInfo != null) {
						resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
					}
					else {
						foreach (var investorFund in investorFunds) {
							errorInfo = InvestorRepository.SaveInvestorFund(investorFund);
							resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
						}
					}
					if (string.IsNullOrEmpty(resultModel.Result)) {
						resultModel.Result += "True||" + (CapitalCallRepository.FindCapitalCallNumber(model.FundId));
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
			ViewData["SubmenuName"] = "Capital Call";
			ViewData["PageName"] = "Receive Capital Call";
			CreateReceiveModel model = new CreateReceiveModel();
			model.CapitalCallId = id ?? 0;
			model.FundId = fundId ?? 0;
			model.CapitalCalls = new List<SelectListItem>();
			model.CapitalCalls.Add(new SelectListItem {
				Value = "0",
				Text = "--Select One--",
				Selected = false
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

				// Attempt to create receive capital call.

				Models.Entity.CapitalCall capitalCall = CapitalCallRepository.FindCapitalCall(model.CapitalCallId);
				if (capitalCall != null) {
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
								else {
									item.ReceivedDate = null;
								}
							}
							if ((item.ReceivedDate ?? Convert.ToDateTime("01/01/1900")).Year <= 1900) {
								item.ReceivedDate = null;
							}
						}
					}
					IEnumerable<ErrorInfo> errorInfo = CapitalCallRepository.SaveCapitalCall(capitalCall);
					resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
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

		#region Manament Fee Calculation

		//GET : /CapitalCall/CalculateManagementFee
		[HttpGet]
		public JsonResult CalculateManagementFee(int fundId, DateTime startDate, DateTime endDate) {
			ManagementFeeDetail detail = new ManagementFeeDetail();
			detail.Tiers = new FlexigridData();

			// Default add three months in start date.
			endDate = startDate.AddMonths(3);

			Models.Entity.Fund fund = CapitalCallRepository.FindFund(fundId);
			if (fund != null) {

				// Find total commitment amount for investor funds by non managing member investor type.
				decimal totalCommittedAmount = fund.InvestorFunds.Where(investorFund => investorFund.InvestorTypeId == (int)Models.Investor.Enums.InvestorType.NonManagingMember).Sum(invfund => invfund.TotalCommitment);

				// Get all fund rate schedule by non managing member investor type.
				FundRateSchedule fundRateSchedule = fund.FundRateSchedules.FirstOrDefault(schedule => schedule.InvestorTypeID == (int)Models.Investor.Enums.InvestorType.NonManagingMember);

				if (totalCommittedAmount > 0 && fundRateSchedule != null) {

					// Find management fee rate schedule.

					ManagementFeeRateSchedule manFeeRateSchedule = FundRepository.FindManagementFeeRateSchedule(fundRateSchedule.RateScheduleID);

					if (manFeeRateSchedule != null) {

						// Get all management fee rate schedule tiers.

						List<ManagementFeeRateScheduleTier> tiers = manFeeRateSchedule.ManagementFeeRateScheduleTiers.Where(feeTier => feeTier.StartDate <= startDate && feeTier.EndDate >= startDate || feeTier.StartDate <= endDate && feeTier.EndDate >= endDate).ToList();
						FlexigridRow tierDetail;

						/* Two types of multiplier type.
						 * 1. Capital Committed (Based on rate %).
						 * 2. Flat Fee (Based on flat fee).
						 */

						switch (tiers.Count) {
							case 1:
								// Calculate base on first quarter.
								tierDetail = new FlexigridRow();

								// Attempt to calculate first quarter (90 days).

								tierDetail.cell.Add(tiers[0].StartDate.ToString("MM/dd/yyyy"));
								tierDetail.cell.Add(tiers[0].EndDate.ToString("MM/dd/yyyy"));
								if (tiers[0].MultiplierTypeID == (int)Models.Fund.Enums.MutiplierType.CapitalCommitted) {
									detail.ManagementFee = CalculateCapitalFee(totalCommittedAmount, tiers[0].Multiplier, 90);
									tierDetail.cell.Add("CapitalCommitted");
									tierDetail.cell.Add(tiers[0].Multiplier.ToString("0.00"));
									tierDetail.cell.Add(string.Empty);
								}
								else {
									detail.ManagementFee = CalculateFlatFee(totalCommittedAmount, tiers[0].Multiplier, 90);
									tierDetail.cell.Add("FlatFee");
									tierDetail.cell.Add(string.Empty);
									tierDetail.cell.Add(tiers[0].Multiplier.ToString("0.00"));
								}
								detail.Tiers.rows.Add(tierDetail);
								break;
							case 2:
								// Calculate base on second quarter.
								tierDetail = new FlexigridRow();

								// Attempt to calculate first start, end date.

								tierDetail.cell.Add(tiers[0].StartDate.ToString("MM/dd/yyyy"));
								tierDetail.cell.Add(tiers[0].EndDate.ToString("MM/dd/yyyy"));
								if (tiers[0].MultiplierTypeID == (int)Models.Fund.Enums.MutiplierType.CapitalCommitted) {
									detail.ManagementFee += CalculateCapitalFee(totalCommittedAmount, tiers[0].Multiplier, (tiers[0].EndDate - startDate).TotalDays);
									tierDetail.cell.Add("CapitalCommitted");
									tierDetail.cell.Add(tiers[0].Multiplier.ToString("0.00"));
									tierDetail.cell.Add(string.Empty);
								}
								else {
									detail.ManagementFee += CalculateFlatFee(totalCommittedAmount, tiers[0].Multiplier, (tiers[0].EndDate - startDate).TotalDays);
									tierDetail.cell.Add("FlatFee");
									tierDetail.cell.Add(string.Empty);
									tierDetail.cell.Add(tiers[0].Multiplier.ToString("0.00"));
								}
								detail.Tiers.rows.Add(tierDetail);

								// Attempt to calculate second start, end date.

								tierDetail = new FlexigridRow();
								tierDetail.cell.Add(tiers[1].StartDate.ToString("MM/dd/yyyy"));
								tierDetail.cell.Add(tiers[1].EndDate.ToString("MM/dd/yyyy"));
								if (tiers[1].MultiplierTypeID == (int)Models.Fund.Enums.MutiplierType.CapitalCommitted) {
									// Reduce 90 days of first quarter.
									detail.ManagementFee += CalculateCapitalFee(totalCommittedAmount, tiers[1].Multiplier, (90 - (tiers[0].EndDate - startDate).TotalDays));
									tierDetail.cell.Add("CapitalCommitted");
									tierDetail.cell.Add(tiers[1].Multiplier.ToString("0.00"));
									tierDetail.cell.Add(string.Empty);
								}
								else {
									// Reduce 90 days of first quarter.
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

		#region Capital Distribution

		//
		// GET: /CapitalCall/NewCapitalDistribution
		[HttpGet]
		public ActionResult NewCapitalDistribution() {
			ViewData["MenuName"] = "Fund Tracker";
			ViewData["SubmenuName"] = "CapitalDistribution";
			ViewData["PageName"] = "CapitalDistribution";
			return View();
		}

		//
		// POST: /CapitalCall/CreateDistribution
		[HttpPost]
		public ActionResult CreateDistribution(FormCollection collection) {
			CreateDistributionModel model = new CreateDistributionModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model, collection);
			if (ModelState.IsValid) {

				// Attempt to create cash distribution.

				CapitalDistribution distribution = new CapitalDistribution();
				CapitalDistributionLineItem item;
				distribution.CapitalDistributionDate = model.CapitalDistributionDate;
				distribution.CapitalDistributionDueDate = model.CapitalDistributionDueDate;

				distribution.PreferredReturn = model.PreferredReturn;
				distribution.ReturnManagementFees = model.ReturnManagementFees;
				distribution.ReturnFundExpenses = model.ReturnFundExpenses;
				distribution.PreferredCatchUp = model.PreferredCatchUp;
				distribution.Profits = model.GPProfits;
				distribution.LPProfits = model.LPProfits;

				distribution.CreatedBy = AppSettings.CreatedByUserId;
				distribution.CreatedDate = DateTime.Now;
				distribution.LastUpdatedBy = AppSettings.CreatedByUserId;
				distribution.LastUpdatedDate = DateTime.Now;
				distribution.DistributionAmount = model.DistributionAmount;
				distribution.DistributionNumber = Convert.ToString(CapitalCallRepository.FindCapitalCallDistributionNumber(model.FundId));
				distribution.FundID = model.FundId;
				distribution.IsManual = false;

				// Get all investor funds.
				List<InvestorFund> investorFunds = CapitalCallRepository.GetAllInvestorFunds(distribution.FundID);

				if (investorFunds != null) {

					// Find non managing member total commitment.
					decimal nonManagingMemberTotalCommitment = investorFunds.Where(fund => fund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.NonManagingMember).Sum(fund => fund.TotalCommitment);
					// Find managing member total commitment.
					decimal managingMemberTotalCommitment = investorFunds.Where(fund => fund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.ManagingMember).Sum(fund => fund.TotalCommitment);
					// Find total commitment.
					decimal totalCommitment = nonManagingMemberTotalCommitment + managingMemberTotalCommitment;

					foreach (var investorFund in investorFunds) {

						// Attempt to create cash distribution of each investor.

						item = new CapitalDistributionLineItem();
						item.CapitalReturn = 0;
						item.CreatedBy = AppSettings.CreatedByUserId;
						item.CreatedDate = DateTime.Now;
						item.LastUpdatedBy = AppSettings.CreatedByUserId;
						item.LastUpdatedDate = DateTime.Now;
						item.InvestorID = investorFund.InvestorID;

						if (distribution.PreferredCatchUp > 0) {
							// ManagingMember investor type only
							if (investorFund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.ManagingMember) {
								item.PreferredCatchUp = distribution.PreferredCatchUp * (investorFund.TotalCommitment / managingMemberTotalCommitment);
							}
						}
						else {
							item.PreferredCatchUp = 0;
						}
						if (distribution.PreferredReturn > 0) {
							// NonManagingMember investor type only
							if (investorFund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.NonManagingMember) {
								item.PreferredReturn = distribution.PreferredReturn * (investorFund.TotalCommitment / nonManagingMemberTotalCommitment);
							}
						}
						else {
							item.PreferredReturn = 0;
						}
						if (distribution.ReturnFundExpenses > 0) {
							// NonManagingMember investor type only
							if (investorFund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.NonManagingMember) {
								item.ReturnFundExpenses = distribution.ReturnFundExpenses * (investorFund.TotalCommitment / nonManagingMemberTotalCommitment);
							}
						}
						else {
							item.ReturnFundExpenses = 0;
						}
						if (distribution.ReturnManagementFees > 0) {
							// NonManagingMember investor type only
							if (investorFund.InvestorTypeId == (int)DeepBlue.Models.Investor.Enums.InvestorType.NonManagingMember) {
								item.ReturnManagementFees = distribution.ReturnManagementFees * (investorFund.TotalCommitment / nonManagingMemberTotalCommitment);
							}
						}
						else {
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
						// Calculate distribution amount of each investor.
						item.DistributionAmount = model.DistributionAmount * (investorFund.TotalCommitment / totalCommitment);
						distribution.CapitalDistributionLineItems.Add(item);
					}
				}
				IEnumerable<ErrorInfo> errorInfo = CapitalCallRepository.SaveCapitalDistribution(distribution);
				resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
				if (string.IsNullOrEmpty(resultModel.Result)) {
					resultModel.Result += "True||" + (CapitalCallRepository.FindCapitalCallDistributionNumber(model.FundId));
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
		// GET: /CapitalCall/CapitalDistributionList
		[HttpGet]
		public ActionResult CapitalDistributionList(int? id) {
			ViewData["MenuName"] = "Fund Tracker";
			ViewData["SubmenuName"] = "Capital Call";
			ViewData["PageName"] = "Capital Distribution List";
			CapitalDistributionListModel model = new CapitalDistributionListModel();
			model.FundId = id ?? 0;
			if (model.FundId > 0) {
				model.FundName = FundRepository.FindFundName(model.FundId);
			}
			return View(model);
		}

		//
		// GET: /CapitalCall/CapitalDistributionDetailList
		[HttpGet]
		public ActionResult CapitalDistributionDetailList(int pageIndex, int pageSize, string sortName, string sortOrder, int fundId) {
			int totalRows = 0;
			List<Models.Entity.CapitalDistribution> capitalCalls = CapitalCallRepository.GetCapitalDistributions(pageIndex, pageSize, sortName, sortOrder, ref totalRows, fundId);
			ViewData["TotalRows"] = totalRows;
			ViewData["PageNo"] = pageIndex;
			return View(capitalCalls);
		}

		#endregion

		#region Manual Capital Distribution
		//
		// POST: /CapitalCall/CreateManualDistribution
		[HttpPost]
		public ActionResult CreateManualDistribution(FormCollection collection) {
			CreateDistributionModel model = new CreateDistributionModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model, collection);
			if (ModelState.IsValid) {

				// Attempt to create cash distribution.

				CapitalDistribution distribution = new CapitalDistribution();
				CapitalDistributionLineItem item;
				distribution.CapitalDistributionDate = model.CapitalDistributionDate;
				distribution.CapitalDistributionDueDate = model.CapitalDistributionDueDate;

				distribution.PreferredReturn = model.PreferredReturn;
				distribution.ReturnManagementFees = model.ReturnManagementFees;
				distribution.ReturnFundExpenses = model.ReturnFundExpenses;
				distribution.PreferredCatchUp = model.PreferredCatchUp;
				distribution.Profits = model.GPProfits;
				distribution.LPProfits = model.LPProfits;

				distribution.CreatedBy = AppSettings.CreatedByUserId;
				distribution.CreatedDate = DateTime.Now;
				distribution.LastUpdatedBy = AppSettings.CreatedByUserId;
				distribution.LastUpdatedDate = DateTime.Now;
				distribution.DistributionAmount = model.DistributionAmount;
				distribution.DistributionNumber = Convert.ToString(CapitalCallRepository.FindCapitalCallDistributionNumber(model.FundId));
				distribution.FundID = model.FundId;
				distribution.IsManual = true;

				int index;

				for (index = 1; index < model.InvestorCount + 1; index++) {
					// Attempt to create cash distribution of each investor.

					item = new CapitalDistributionLineItem();
					item.CapitalReturn = 0;
					item.CreatedBy = AppSettings.CreatedByUserId;
					item.CreatedDate = DateTime.Now;
					item.LastUpdatedBy = AppSettings.CreatedByUserId;
					item.LastUpdatedDate = DateTime.Now;
					item.InvestorID = DataTypeHelper.ToInt32(collection[index.ToString() + "_" + "InvestorId"]);
					item.DistributionAmount = DataTypeHelper.ToDecimal(collection[index.ToString() + "_" + "DistributionAmount"]);
					item.ReturnManagementFees = DataTypeHelper.ToDecimal(collection[index.ToString() + "_" + "ReturnManagementFees"]);
					item.ReturnFundExpenses = DataTypeHelper.ToDecimal(collection[index.ToString() + "_" + "ReturnFundExpenses"]);
					item.Profits = DataTypeHelper.ToDecimal(collection[index.ToString() + "_" + "GPProfits"]);
					item.PreferredReturn = DataTypeHelper.ToDecimal(collection[index.ToString() + "_" + "PreferredReturn"]);
					if (item.InvestorID > 0) {
						distribution.CapitalDistributionLineItems.Add(item);
					}
				}
				if (distribution.CapitalDistributionLineItems.Count == 0) {
					ModelState.AddModelError("InvestorCount", "Select any one investor");
				}
				else {
					IEnumerable<ErrorInfo> errorInfo = CapitalCallRepository.SaveCapitalDistribution(distribution);
					resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
					if (string.IsNullOrEmpty(resultModel.Result)) {
						resultModel.Result += "True||" + (CapitalCallRepository.FindCapitalCallDistributionNumber(model.FundId));
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

		#region Capital Call Detail

		//
		//GET : /CapitalCall/Detail
		[HttpGet]
		public ActionResult Detail(int? fundId, int? typeId) {
			ViewData["MenuName"] = "Fund Tracker";
			ViewData["SubmenuName"] = "CapitalCall";
			ViewData["PageName"] = "Detail";
			if ((typeId ?? 0) == 0) {
				typeId = (int)DetailType.CapitalCall;
			}
			DetailModel model = new DetailModel();
			model.FundId = fundId ?? 0;
			model.DetailType = (DetailType)typeId;
			return View("Detail", model);
		}

		//
		//GET : /CapitalCall/FindDetail
		[HttpGet]
		public JsonResult FindDetail(int fundId) {
			return Json(CapitalCallRepository.FindDetail(fundId), JsonRequestBehavior.AllowGet);
		}

		//
		//GET : /CapitalCall/GetCapitalCallInvestors
		[HttpGet]
		public JsonResult GetCapitalCallInvestors(int capitalCallId) {
			return Json(new { Investors = CapitalCallRepository.GetCapitalCallInvestors(capitalCallId) }, JsonRequestBehavior.AllowGet);
		}

		//
		//GET : /CapitalCall/FundDetail
		[HttpGet]
		public JsonResult FundDetail(int id) {
			return Json(CapitalCallRepository.FindFundDetail(id), JsonRequestBehavior.AllowGet);
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
		// GET: /CapitalCall/GetCapitalCallList
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
				int index = 0;
				foreach (var item in capitalCall.CapitalCallLineItems) {
					index++;
					model.Items.Add(new CapitalCallLineItemDetail {
						Index = index,
						InvestorName = item.Investor.InvestorName,
						CapitalAmountCalled = item.CapitalAmountCalled,
						InvestedAmountInterest = item.InvestedAmountInterest ?? 0,
						CapitalCallLineItemId = item.CapitalCallLineItemID,
						InvestmentAmount = item.InvestedAmountInterest ?? 0,
						ManagementFeeInterest = item.ManagementFeeInterest ?? 0,
						ManagementFees = item.ManagementFees ?? 0,
						Received = (item.ReceivedDate.HasValue ? true : false),
						ReceivedDate = ((item.ReceivedDate ?? Convert.ToDateTime("01/01/1900")).Year <= 1900 ? string.Empty : (item.ReceivedDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy"))
					});
				}
			}
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		//
		//GET : /CapitalCall/List
		public ActionResult List(int? id) {
			ViewData["MenuName"] = "Fund Tracker";
			ViewData["SubmenuName"] = "Capital Call";
			ViewData["PageName"] = "Capital Call List";
			ListModel model = new ListModel();
			model.FundId = id ?? 0;
			if (model.FundId > 0) {
				FundDetail fundDetail = CapitalCallRepository.FindFundDetail(model.FundId);
				if (fundDetail != null) {
					model.FundName = fundDetail.FundName;
				}
			}
			return View(model);
		}

		#endregion

		//
		//GET : /CapitalCall/Result
		public ActionResult Result() {
			return View();
		}
	}
}
