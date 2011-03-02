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
    public class TransactionValidDataTest : InvestorFundTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultInvestorFund, true);
			this.ServiceErrors = DefaultInvestorFund.Save();
        }

		[Test]
		public void create_a_new_investor_fund_with_valid_fundid_throws_error() {
			Assert.IsTrue(IsPropertyValid("FundID"));
		}

		[Test]
		public void create_a_new_investor_fund_with_created_by_throws_error() {
			Assert.IsTrue(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_investor_fund_with_created_date_throws_error() {
			Assert.IsTrue(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_investor_fund_with_committed_date_throws_error() {
			Assert.IsTrue(IsPropertyValid("CommittedDate"));
		}

		[Test]
		public void create_a_new_investor_fund_with_tota_commitment_throws_error() {
			Assert.IsTrue(IsPropertyValid("TotalCommitment"));
		}
      
    }
}