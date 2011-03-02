using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Transaction {
    public class InvestorFundTransactionTest : Base {
        public DeepBlue.Models.Entity.InvestorFundTransaction DefaultInvestorFundTransaction { get; set; }

        public Mock<IInvestorFundTransactionService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IInvestorFundTransactionService>();

			DefaultInvestorFundTransaction = new DeepBlue.Models.Entity.InvestorFundTransaction(MockService.Object);
			MockService.Setup(x => x.SaveInvestorFundTransaction(It.IsAny<DeepBlue.Models.Entity.InvestorFundTransaction>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.InvestorFundTransaction investorFundTransaction, bool ifValid) {
			RequiredFieldDataMissing(investorFundTransaction, ifValid);
		}

		#region InvestorContact
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.InvestorFundTransaction investorFundTransaction, bool ifValidData) {
			if (ifValidData) {
				investorFundTransaction.FundClosingID = 1;
				investorFundTransaction.CreatedBy = 1;
				investorFundTransaction.CreatedDate = DateTime.Now;
				investorFundTransaction.OtherInvestorID = 1;
				investorFundTransaction.TransactionTypeID = 1;
				investorFundTransaction.Amount = 1;
			} else {
				investorFundTransaction.FundClosingID = 0;
				investorFundTransaction.CreatedBy = 0;
				investorFundTransaction.CreatedDate = DateTime.MinValue;
				investorFundTransaction.OtherInvestorID = 0;
				investorFundTransaction.TransactionTypeID = 0;
				investorFundTransaction.Amount = 0;
			}
		}
		#endregion
 
    }
}