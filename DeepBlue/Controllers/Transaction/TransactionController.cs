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
		// GET: /Transaction/Edit
		public ActionResult Edit(int id) {
			return View(InvestorRepository.FindInvestorFund(id));
		}

	}
}
