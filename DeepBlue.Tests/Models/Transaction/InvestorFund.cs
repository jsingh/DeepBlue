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
    public class InvestorFundTest : Base {
        public DeepBlue.Models.Entity.InvestorFund DefaultInvestorFund { get; set; }

        public Mock<IInvestorFundService> MockService { get; set; }

        [SetUp]
        public override void Setup() {
            base.Setup();

            // Spin up mock repository and attach to controller
            MockService = new Mock<IInvestorFundService>();

			DefaultInvestorFund = new DeepBlue.Models.Entity.InvestorFund(MockService.Object);
			MockService.Setup(x => x.SaveInvestorFund(It.IsAny<DeepBlue.Models.Entity.InvestorFund>()));
        }

        protected bool IsPropertyValid(string propertyName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsModelValid(out errorMsg, out errorCount, propertyName);
        }

		protected void Create_Data(DeepBlue.Models.Entity.InvestorFund investorFund, bool ifValid) {
			RequiredFieldDataMissing(investorFund, ifValid);
		}

		#region InvestorContact
		private void RequiredFieldDataMissing(DeepBlue.Models.Entity.InvestorFund investorFund, bool ifValidData) {
			if (ifValidData) {
				investorFund.FundID = 1;
				investorFund.CreatedBy = 1;
				investorFund.CreatedDate = DateTime.Now;
				investorFund.TotalCommitment = 1;
			} else {
				investorFund.FundID = 0;
				investorFund.CreatedBy = 0;
				investorFund.CreatedDate = DateTime.MinValue;
				investorFund.TotalCommitment = 0;
			}
		}
		#endregion
 
    }
}