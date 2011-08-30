using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Transaction;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Investor;
using DeepBlue.Models.Entity;
using DeepBlue.Controllers.Admin;


namespace DeepBlue.Controllers.Transaction {
	public class TransactionController : BaseController {

		public ITransactionRepository TransactionRepository { get; set; }
		public IInvestorRepository InvestorRepository { get; set; }
		public IAdminRepository AdminRepository { get; set; }

		public TransactionController()
			: this(new TransactionRepository(), new InvestorRepository(), new AdminRepository()) {
		}

		public TransactionController(ITransactionRepository transactionRepository, IInvestorRepository investorRepository, IAdminRepository adminRepository) {
			TransactionRepository = transactionRepository;
			InvestorRepository = investorRepository;
			AdminRepository = adminRepository;
		}

		//
		// GET: /Transaction/

		public ActionResult Index() {
			ViewData["MenuName"] = "Investor";
			return View();
		}


		//
		// GET: /Transaction/New

		public ActionResult New() {
			ViewData["MenuName"] = "Investor";
			ViewData["PageName"] = "Investor Commitment";
			CreateModel model = new CreateModel();
			List<SelectListItem> emptyList = SelectListFactory.GetEmptySelectList();
			emptyList.Add(new SelectListItem { Text = "Add Fund Close", Value = "-1" });
			model.FundClosings = emptyList;
			model.InvestorTypes = SelectListFactory.GetInvestorTypeSelectList(AdminRepository.GetAllInvestorTypes());
			model.InvestorId = 0;
			model.EditModel = new EditModel();
			model.EditModel.InvestorTypes = SelectListFactory.GetInvestorTypeSelectList(AdminRepository.GetAllInvestorTypes());
			return View(model);
		}

		//
		// GET: /Transaction/CreateInvestorFund

		public ActionResult CreateInvestorFund(FormCollection collection) {
			CreateModel model = new CreateModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				InvestorFund investorFund = InvestorRepository.FindInvestorFund(model.InvestorId, model.FundId);
				if (investorFund == null) {
					//Create new fund
					investorFund = new InvestorFund();
					investorFund.CreatedBy = Authentication.CurrentUser.UserID;
					investorFund.CreatedDate = DateTime.Now;
					investorFund.FundID = model.FundId;
					investorFund.InvestorTypeId = model.InvestorTypeId;
					investorFund.TotalCommitment = model.TotalCommitment;
					investorFund.UnfundedAmount = model.TotalCommitment;
					investorFund.InvestorID = model.InvestorId;
				}
				else {
					investorFund.TotalCommitment += model.TotalCommitment;
					investorFund.UnfundedAmount += model.TotalCommitment;
				}
				investorFund.LastUpdatedBy = Authentication.CurrentUser.UserID;
				investorFund.LastUpdatedDate = DateTime.Now;
				IEnumerable<ErrorInfo> errorInfo = InvestorRepository.SaveInvestorFund(investorFund);
				if (errorInfo == null) {
					// Create new investor fund transaction
					InvestorFundTransaction investorFundTransaction = new InvestorFundTransaction();
					investorFundTransaction.Amount = model.TotalCommitment;
					investorFundTransaction.CreatedBy = Authentication.CurrentUser.UserID;
					investorFundTransaction.CreatedDate = DateTime.Now;
					investorFundTransaction.FundClosingID = model.FundClosingId;
					investorFundTransaction.IsAgreementSigned = false;
					investorFundTransaction.OtherInvestorID = null;
					investorFundTransaction.TransactionTypeID = (int)DeepBlue.Models.Transaction.Enums.TransactionType.OriginalCommitment;
					investorFundTransaction.Notes = string.Empty;
					investorFundTransaction.InvestorFundID = investorFund.InvestorFundID;
					errorInfo = InvestorRepository.SaveInvestorFundTransaction(investorFundTransaction);
				}
				resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
			}
			else {
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
		// GET: /Transaction/List
		[HttpGet]
		public ActionResult List(int id) {
			return View(InvestorRepository.FindInvestorFunds(id));
		}


		//
		// GET: /Transaction/FundClosingList
		[HttpGet]
		public JsonResult FundClosingList(int fundId, int investorId) {
			List<FundClosing> fundClosings = TransactionRepository.GetAllFundClosings(fundId);
			InvestorFund investorFund = InvestorRepository.FindInvestorFund(investorId, fundId);
			FundDetail fundDetail = new FundDetail();
			if (investorFund != null) {
				if ((investorFund.InvestorTypeId ?? 0) > 0) {
					fundDetail.InvestorTypeId = investorFund.InvestorTypeId ?? 0;
					fundDetail.InvestorTypeName = investorFund.InvestorType.InvestorTypeName;
				}
			}
			foreach (var fundClose in fundClosings) {
				fundDetail.FundClosingDetails.Add(new FundClosingDetail(fundClose.FundClosingID, fundClose.Name + " - " + (fundClose.FundClosingDate ?? Convert.ToDateTime("01/01/1900")).ToString("MM/dd/yyyy")));
			}
			return Json(fundDetail, JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Transaction/InvestorType
		[HttpGet]
		public JsonResult InvestorType(int investorId, int fundId) {
			InvestorFund investorFund = InvestorRepository.FindInvestorFund(investorId, fundId);
			InvestorTypeDetail investorTypeDetail = new InvestorTypeDetail();
			if (investorFund != null) {
				if ((investorFund.InvestorTypeId ?? 0) > 0) {
					investorTypeDetail.InvestorTypeId = investorFund.InvestorTypeId ?? 0;
					investorTypeDetail.InvestorTypeName = investorFund.InvestorType.InvestorTypeName;
				}
			}
			return Json(investorTypeDetail, JsonRequestBehavior.AllowGet);
		}


		//
		// GET: /Transaction/Success
		public ActionResult Success() {
			return View();
		}

		// GET: /Transaction/Error
		public ActionResult Error() {
			return View();
		}

		//
		// GET: /Transaction/Edit/5
		public ActionResult Edit(int id) {
			EditModel model = new EditModel();
			InvestorFund investorFund = InvestorRepository.FindInvestorFund(id);
			if (investorFund != null) {
				model.InvestorName = investorFund.Investor.InvestorName;
				model.InvestorId = investorFund.Investor.InvestorID;
				model.InvestorFundId = investorFund.InvestorFundID;
				model.OriginalCommitmentAmount = investorFund.TotalCommitment;
				model.UnfundedAmount = investorFund.UnfundedAmount ?? 0;
				model.Date = DateTime.Now;
				model.CounterPartyInvestorId = 0;
				model.CounterPartyInvestorName = string.Empty;
				model.TransactionTypeId = (int)DeepBlue.Models.Transaction.Enums.TransactionType.Sell;
				model.InvestorTypes = SelectListFactory.GetInvestorTypeSelectList(AdminRepository.GetAllInvestorTypes());
				model.FundId = investorFund.FundID;
			}
			return View(model);
		}

		[HttpGet]
		public JsonResult FindInvestorFundDetail(int id) {
			return Json(TransactionRepository.FindInvestorFundDetail(id), JsonRequestBehavior.AllowGet);
		}

		//POST: /Transaction/CreateFundTransaction
		[HttpPost]
		public ActionResult CreateFundTransaction(FormCollection collection) {
			EditModel model = new EditModel();
			ResultModel resultModel = new ResultModel();
			this.TryUpdateModel(model);
			IEnumerable<ErrorInfo> errorInfo = null;
			if (ModelState.IsValid) {
				InvestorFund investorFund = InvestorRepository.FindInvestorFund(model.InvestorFundId);
				//Seller investor fund transaction
				InvestorFundTransaction sellerInvestorFundTransaction = new InvestorFundTransaction();
				sellerInvestorFundTransaction.Amount = model.CommitmentAmount;
				sellerInvestorFundTransaction.CreatedBy = Authentication.CurrentUser.UserID;
				sellerInvestorFundTransaction.CreatedDate = DateTime.Now;
				sellerInvestorFundTransaction.FundClosingID = null;
				sellerInvestorFundTransaction.IsAgreementSigned = false;
				sellerInvestorFundTransaction.OtherInvestorID = null;
				sellerInvestorFundTransaction.TransactionTypeID = (int)DeepBlue.Models.Transaction.Enums.TransactionType.Sell;
				sellerInvestorFundTransaction.CommittedDate = model.Date;
				sellerInvestorFundTransaction.Notes = model.Notes;
				sellerInvestorFundTransaction.InvestorFundID = investorFund.InvestorFundID;

				errorInfo = InvestorRepository.SaveInvestorFundTransaction(sellerInvestorFundTransaction);

				if (errorInfo == null) {

					InvestorFund counterPartyInvestorFund = InvestorRepository.FindInvestorFund(model.CounterPartyInvestorId, investorFund.FundID);

					if (counterPartyInvestorFund == null) {
						counterPartyInvestorFund = new InvestorFund(); //Create new investor fund
						counterPartyInvestorFund.CreatedBy = Authentication.CurrentUser.UserID;
						counterPartyInvestorFund.CreatedDate = DateTime.Now;
						counterPartyInvestorFund.FundID = investorFund.FundID;
						counterPartyInvestorFund.InvestorID = model.CounterPartyInvestorId;
						counterPartyInvestorFund.LastUpdatedBy = Authentication.CurrentUser.UserID;
						counterPartyInvestorFund.LastUpdatedDate = DateTime.Now;
						counterPartyInvestorFund.TotalCommitment = model.CommitmentAmount;
						counterPartyInvestorFund.UnfundedAmount = model.CommitmentAmount;
						counterPartyInvestorFund.InvestorTypeId = investorFund.InvestorTypeId;
					}
					else {
						counterPartyInvestorFund.TotalCommitment += model.CommitmentAmount;
						counterPartyInvestorFund.UnfundedAmount += model.CommitmentAmount;
						counterPartyInvestorFund.LastUpdatedBy = Authentication.CurrentUser.UserID;
						counterPartyInvestorFund.LastUpdatedDate = DateTime.Now;
					}

					InvestorFundTransaction counterPartyFundTransaction = new InvestorFundTransaction();
					counterPartyFundTransaction.Amount = model.CommitmentAmount;
					counterPartyFundTransaction.CreatedBy = Authentication.CurrentUser.UserID;
					counterPartyFundTransaction.CreatedDate = DateTime.Now;
					counterPartyFundTransaction.FundClosingID = null;
					counterPartyFundTransaction.IsAgreementSigned = false;
					counterPartyFundTransaction.OtherInvestorID = null;
					counterPartyFundTransaction.TransactionTypeID = (int)DeepBlue.Models.Transaction.Enums.TransactionType.Buy;
					counterPartyFundTransaction.CommittedDate = model.Date;
					counterPartyFundTransaction.Notes = model.Notes;

					// Save counter party investor fund
					errorInfo = InvestorRepository.SaveInvestorFund(counterPartyInvestorFund);
					if (errorInfo == null) {
						counterPartyFundTransaction.InvestorFundID = counterPartyInvestorFund.InvestorFundID;
						errorInfo = InvestorRepository.SaveInvestorFundTransaction(counterPartyFundTransaction);
					}
				}
				if (errorInfo == null) {
					// Update unfunded amount
					investorFund.UnfundedAmount -= model.CommitmentAmount;
					investorFund.TotalCommitment -= model.CommitmentAmount;
					errorInfo = InvestorRepository.SaveInvestorFund(investorFund);
				}
				resultModel.Result += ValidationHelper.GetErrorInfo(errorInfo);
			}
			else {
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

		[HttpGet]
		public JsonResult FindCommitmentAmount(int id) {
			EditCommitmentAmountModel editModel = new EditCommitmentAmountModel();
			InvestorFund investorFund = InvestorRepository.FindInvestorFund(id);
			editModel.InvestorFundId = investorFund.InvestorFundID;
			editModel.CommitmentAmount = investorFund.TotalCommitment;
			editModel.UnfundedAmount = investorFund.UnfundedAmount ?? 0;
			return Json(editModel, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public string IsFundAvailable(int fundClosingId, int fundId) {
			return string.Empty;
		}

		[HttpPost]
		public ActionResult UpdateCommitmentAmount(FormCollection collection) {
			EditCommitmentAmountModel editModel = new EditCommitmentAmountModel();
			this.TryUpdateModel(editModel);
			ResultModel resultModel = new ResultModel();
			if (ModelState.IsValid) {
				InvestorFund investorFund = InvestorRepository.FindInvestorFund(editModel.InvestorFundId);
				investorFund.UnfundedAmount = editModel.CommitmentAmount;
				investorFund.TotalCommitment = editModel.CommitmentAmount;
				InvestorRepository.SaveInvestorFund(investorFund);
			}
			else {
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
	}
}
