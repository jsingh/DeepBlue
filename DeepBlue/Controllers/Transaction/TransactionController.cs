using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeepBlue.Models.Transaction;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Investor;
using DeepBlue.Models.Entity;


namespace DeepBlue.Controllers.Transaction {
	public class TransactionController : Controller {

		public ITransactionRepository TransactionRepository { get; set; }
		public IInvestorRepository InvestorRepository { get; set; }

		public TransactionController()
			: this(new TransactionRepository(), new InvestorRepository()) {
		}

		public TransactionController(ITransactionRepository transactionRepository, IInvestorRepository investorRepository) {
			TransactionRepository = transactionRepository;
			InvestorRepository = investorRepository;
		}


		//
		// GET: /Transaction/

		public ActionResult Index() {
			ViewData["MenuName"] = "Investor";
			return View();
		}


		//
		// GET: /Transaction/New

		public ActionResult New(int? id) {
			ViewData["MenuName"] = "Investor";
			CreateModel model = new CreateModel();
			DeepBlue.Models.Entity.Investor investor = InvestorRepository.FindInvestor(id ?? 0);
			if (investor != null) {
				model.InvestorName = investor.InvestorName;
				model.DisplayName = investor.Alias;
			}
			model.FundNames = SelectListFactory.GetFundSelectList(TransactionRepository.GetAllFundNames());
			model.FundClosings = SelectListFactory.GetFundClosingSelectList(TransactionRepository.GetAllFundClosings());
			model.InvestorTypes = SelectListFactory.GetInvestorTypeSelectList(InvestorRepository.GetAllInvestorTypes());
			model.InvestorId = id ?? 0;
			return View(model);
		}

		//
		// GET: /Transaction/New

		public ActionResult CreateInvestorFund(FormCollection collection) {
			CreateModel model = new CreateModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				InvestorFund investorFund = new InvestorFund();
				investorFund.CommittedDate = Convert.ToDateTime(model.CommittedDate);
				investorFund.CreatedBy = AppSettings.CreatedByUserId;
				investorFund.CreatedDate = DateTime.Now;
				investorFund.FundID = model.FundId;
				investorFund.InvestorTypeId = model.InvestorTypeId;
				investorFund.LastUpdatedBy = 0;
				investorFund.LastUpdatedDate = DateTime.Now;
				investorFund.TotalCommitment = model.TotalCommitment;
				investorFund.UnfundedAmount = model.TotalCommitment;
				investorFund.InvestorID = model.InvestorId;
				// Create new investor fund transaction
				InvestorFundTransaction investorFundTransaction = new InvestorFundTransaction();
				investorFundTransaction.Amount = 0;
				investorFundTransaction.CreatedBy = AppSettings.CreatedByUserId;
				investorFundTransaction.CreatedDate = DateTime.Now;
				investorFundTransaction.FundClosingID = model.FundClosingId;
				investorFundTransaction.IsAgreementSigned = model.IsAgreementSigned;
				investorFundTransaction.OtherInvestorID = 0;
				investorFundTransaction.TransactionTypeID = 0;
				investorFund.InvestorFundTransactions.Add(investorFundTransaction);

				InvestorRepository.SaveInvestorFund(investorFund);
				return View("CreateInvestorFundSuccess");
			} else {
				return View("New", model);
			}
		}

		//
		// GET: /Transaction/List
		[HttpGet]
		public ActionResult List(int id) {
			return View(InvestorRepository.FindInvestorFunds(id));
		}

		//
		// GET: /Transaction/CreateInvestorFundSuccess
		public ActionResult CreateInvestorFundSuccess() {
			return View();
		}

		//
		// GET: /Transaction/Edit/5
		public ActionResult Edit(int id) {
			EditModel editModel = new EditModel();
			InvestorFundTransaction investorFundTrasaction = InvestorRepository.FindInvestorFundTransaction(id);
			if (investorFundTrasaction != null) {
				editModel.InvestorName = investorFundTrasaction.InvestorFund.Investor.InvestorName;
				editModel.InvestorId = investorFundTrasaction.InvestorFund.Investor.InvestorID;
				editModel.InvestorFundId = investorFundTrasaction.InvestorFundID;
				editModel.OriginalCommitmentAmount = investorFundTrasaction.InvestorFund.TotalCommitment;
				editModel.UnfundedAmount = investorFundTrasaction.InvestorFund.UnfundedAmount ?? 0;
				editModel.InvestorFundTransactionId = investorFundTrasaction.InvestorFundTransactionID;
				editModel.Date = investorFundTrasaction.CreatedDate;
				editModel.CounterPartyInvestorId = (int)investorFundTrasaction.OtherInvestorID;
				if (editModel.CounterPartyInvestorId > 0) {
					DeepBlue.Models.Entity.Investor otherInvestor = InvestorRepository.FindInvestor(editModel.CounterPartyInvestorId);
					if (otherInvestor != null) {
						editModel.CounterPartyInvestorName = otherInvestor.InvestorName;
					}
				}
				editModel.TransactionTypeId = investorFundTrasaction.TransactionTypeID;
				if (editModel.TransactionTypeId <= 0)
					editModel.TransactionTypeId = (int)DeepBlue.Models.Transaction.Enums.TransactionType.Buy;
			}
			return View(editModel);
		}

		//POST: /Transaction/Update
		[HttpPost]
		public ActionResult Update(FormCollection collection) {
			EditModel model = new EditModel();
			this.TryUpdateModel(model);
			if (ModelState.IsValid) {
				InvestorFund investorFund = InvestorRepository.FindInvestorFund(model.InvestorFundId);
				InvestorFundTransaction investorFundTrasaction = investorFund.InvestorFundTransactions.SingleOrDefault(transaction => transaction.InvestorFundTransactionID == model.InvestorFundTransactionId);
				if (investorFundTrasaction != null) {
					if (model.CommitmentAmount > investorFund.UnfundedAmount) {
						ModelState.AddModelError("CommitmentAmount", "Transaction Amount should be less than Unfunded Commitment Amount.");
					} else {
						//Seller investor fund transaction
						InvestorFundTransaction sellerInvestorFundTransaction = new InvestorFundTransaction();

						sellerInvestorFundTransaction.Amount = model.CommitmentAmount;
						sellerInvestorFundTransaction.CreatedBy = AppSettings.CreatedByUserId;
						sellerInvestorFundTransaction.CreatedDate = DateTime.Now;
						sellerInvestorFundTransaction.FundClosingID = investorFundTrasaction.FundClosingID;
						sellerInvestorFundTransaction.IsAgreementSigned = false;
						sellerInvestorFundTransaction.OtherInvestorID = model.CounterPartyInvestorId;
						sellerInvestorFundTransaction.TransactionTypeID = (int)DeepBlue.Models.Transaction.Enums.TransactionType.Sell;
						investorFund.InvestorFundTransactions.Add(sellerInvestorFundTransaction);

						InvestorFund counterPartyInvestorFund = InvestorRepository.FindInvestorFund(model.CounterPartyInvestorId, model.InvestorFundId);
						if (counterPartyInvestorFund == null) {
							counterPartyInvestorFund = new InvestorFund(); //Create new investor fund
							counterPartyInvestorFund.CommittedDate = Convert.ToDateTime(model.Date);
							counterPartyInvestorFund.CreatedBy = AppSettings.CreatedByUserId;
							counterPartyInvestorFund.CreatedDate = DateTime.Now;
							counterPartyInvestorFund.FundID = investorFund.FundID;
							counterPartyInvestorFund.InvestorID = model.CounterPartyInvestorId;
							counterPartyInvestorFund.LastUpdatedBy = AppSettings.CreatedByUserId;
							counterPartyInvestorFund.LastUpdatedDate = DateTime.Now;
							counterPartyInvestorFund.TotalCommitment = model.CommitmentAmount;
							counterPartyInvestorFund.UnfundedAmount = model.CommitmentAmount;
							counterPartyInvestorFund.InvestorTypeId = 0;
						} else {
							counterPartyInvestorFund.TotalCommitment += model.CommitmentAmount;
						}

						InvestorFundTransaction counterPartyFundTransaction = new InvestorFundTransaction();
						counterPartyFundTransaction.Amount = model.CommitmentAmount;
						counterPartyFundTransaction.CreatedBy = AppSettings.CreatedByUserId;
						counterPartyFundTransaction.CreatedDate = DateTime.Now;
						counterPartyFundTransaction.FundClosingID = investorFundTrasaction.FundClosingID;
						counterPartyFundTransaction.IsAgreementSigned = false;
						counterPartyFundTransaction.OtherInvestorID = 0;
						counterPartyFundTransaction.TransactionTypeID = (int)DeepBlue.Models.Transaction.Enums.TransactionType.Buy;
						counterPartyInvestorFund.InvestorFundTransactions.Add(counterPartyFundTransaction);

						// Save counter party investor fund
						IEnumerable<ErrorInfo> errorInfo = InvestorRepository.SaveInvestorFund(counterPartyInvestorFund);
						if (errorInfo == null) {
							// Update unfunded amount
							investorFund.UnfundedAmount -= model.CommitmentAmount;
							InvestorRepository.UpdateInvestorFund(investorFund);
						}
						model.TransactionSuccess = true;
					}
				}
			}
			return View("Edit", model);
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


		[HttpPost]
		public bool UpdateCommitmentAmount(FormCollection collection) {
			EditCommitmentAmountModel editModel = new EditCommitmentAmountModel();
			this.TryUpdateModel(editModel);
			if (ModelState.IsValid) {
				InvestorFund investorFund = InvestorRepository.FindInvestorFund(editModel.InvestorFundId);
				if (investorFund.TotalCommitment < editModel.CommitmentAmount)
					investorFund.UnfundedAmount += (editModel.CommitmentAmount - investorFund.TotalCommitment);
				investorFund.TotalCommitment = editModel.CommitmentAmount;
				InvestorRepository.UpdateInvestorFund(investorFund);
				return true;
			} else {
				return false;
			}
		}
	}
}
