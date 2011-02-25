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
			ViewData["MenuName"] = "Transaction";
			return View();
		}


		//
		// GET: /Transaction/New

		public ActionResult New(int id) {
			ViewData["MenuName"] = "Transaction";
			CreateTransactionModel model = new CreateTransactionModel();
			DeepBlue.Models.Entity.Investor investor = InvestorRepository.FindInvestor(id);
			if (investor != null) {
				model.InvestorName = investor.InvestorName;
				model.DisplayName = investor.Alias;
			}
			model.FundNames = SelectListFactory.GetFundSelectList(TransactionRepository.GetAllFundNames());
			model.FundClosings = SelectListFactory.GetFundClosingSelectList(TransactionRepository.GetAllFundClosings());
			model.InvestorTypes = SelectListFactory.GetInvestorTypeSelectList(InvestorRepository.GetAllInvestorTypes());
			model.InvestorId = id;
			return View(model);
		}

		//
		// GET: /Transaction/New

		public bool CreateInvestorFund(CreateTransactionModel model) {
			InvestorFund investorFund = new InvestorFund();
			investorFund.CommittedDate = Convert.ToDateTime(model.CommittedDate);
			investorFund.CreatedBy = 0;
			investorFund.CreatedDate = DateTime.Now;
			investorFund.FundID = model.FundId;
			investorFund.InvestorTypeId = model.InvestorTypeId;
			investorFund.LastUpdatedBy = 0;
			investorFund.LastUpdatedDate = DateTime.Now;
			investorFund.TotalCommitment = model.TotalCommitment;
			investorFund.UnfundedAmount = 0;
			investorFund.InvestorID = model.InvestorId;
			// Create new investor fund transaction
			InvestorFundTransaction investorFundTransaction = new InvestorFundTransaction();
			investorFundTransaction.Amount = 0;
			investorFundTransaction.CreatedBy = 0;
			investorFundTransaction.CreatedDate = DateTime.Now;
			investorFundTransaction.FundClosingID = model.FundClosingId;
			investorFundTransaction.IsAgreementSigned = model.IsAgreementSigned;
			investorFundTransaction.OtherInvestorID = 0;
			investorFundTransaction.TransactionTypeID = 0;
			investorFund.InvestorFundTransactions.Add(investorFundTransaction);

			TransactionRepository.AddInvestorFund(investorFund);
			TransactionRepository.Save();
			return true;
		}

		//
		// GET: /Transaction/List
		[HttpGet]
		public ActionResult List() {
			return View(InvestorRepository.FindInvestorFunds(Convert.ToInt32(Request.QueryString["id"])));
		}

		//
		// GET: /Transaction/Edit/5
		public ActionResult Edit(int? id) {
			ViewData["SaveTransaction"] = false;
			EditModel editModel = new EditModel();
			if (id.HasValue) {
				InvestorFundTransaction investorFundTrasaction = InvestorRepository.FindInvestorFundTransaction(id ?? 0);
				editModel.InvestorName = investorFundTrasaction.InvestorFund.Investor.InvestorName;
				editModel.InvestorId = investorFundTrasaction.InvestorFund.Investor.InvestorID;
				editModel.InvestorFundId = investorFundTrasaction.InvestorFundID;
				editModel.OriginalCommitmentAmount = investorFundTrasaction.InvestorFund.TotalCommitment;
				editModel.InvestorFundTransactionId = investorFundTrasaction.InvestorFundTransactionID;
				editModel.CommitmentAmount = (decimal)investorFundTrasaction.Amount;
				editModel.Date = investorFundTrasaction.CreatedDate.ToString("MM/dd/yyyy");
				if (investorFundTrasaction.TransactionTypeID == (int)DeepBlue.Models.Transaction.Enums.TransactionType.Split) {
					editModel.OtherInvestorCommitmentAmount = (decimal)investorFundTrasaction.Amount;
				}
				editModel.OtherInvestorId = (int)investorFundTrasaction.OtherInvestorID;
				if (editModel.OtherInvestorId > 0) {
					DeepBlue.Models.Entity.Investor otherInvestor = InvestorRepository.FindInvestor(editModel.OtherInvestorId);
					if (otherInvestor != null) {
						editModel.OtherInvestorName = otherInvestor.InvestorName;
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
			InvestorFund investorFund = InvestorRepository.FindInvestorFund(Convert.ToInt32(collection["InvestorFundId"]));
			InvestorFundTransaction investorFundTrasaction = investorFund.InvestorFundTransactions.Single(transaction => transaction.InvestorFundTransactionID == Convert.ToInt32(collection["InvestorFundTransactionId"]));
			if (investorFundTrasaction != null) {
				investorFundTrasaction.TransactionTypeID = Convert.ToInt32(collection["TransactionType"]);
				investorFundTrasaction.CreatedDate = Convert.ToDateTime(collection["Date"]);
				if (investorFundTrasaction.TransactionTypeID == (int)DeepBlue.Models.Transaction.Enums.TransactionType.Split) {
					investorFundTrasaction.InvestorFund.TotalCommitment = Convert.ToDecimal(collection["CommitmentAmount"]);
					investorFundTrasaction.Amount = Convert.ToDecimal(collection["OtherInvestorCommitmentAmount"]);
					investorFundTrasaction.OtherInvestorID = Convert.ToInt32(collection["OtherInvestorId"]);
				} else {
					investorFundTrasaction.Amount = Convert.ToDecimal(collection["CommitmentAmount"]);
					investorFundTrasaction.OtherInvestorID = 0;
				}
			}
			InvestorRepository.Save();
			ViewData["SaveTransaction"] = true;
			return RedirectToAction("Edit", "Transaction");
		}

	}
}
