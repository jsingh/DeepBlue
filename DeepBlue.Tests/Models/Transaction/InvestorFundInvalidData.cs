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
	public class TransactionFundInvalidData : InvestorFundTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultInvestorFund, false);
			this.ServiceErrors = DefaultInvestorFund.Save();
        }

        [Test]
        public void create_a_new_investor_fund_without_valid_fundid_throws_error() {
			Assert.IsFalse(IsPropertyValid("FundID"));
        }

        [Test]
        public void create_a_new_investor_fund_without_created_by_throws_error() {
			Assert.IsFalse(IsPropertyValid("CreatedBy"));
        }

		[Test]
		public void create_a_new_investor_fund_without_created_date_throws_error() {
			Assert.IsFalse(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_investor_fund_without_committed_date_throws_error() {
			Assert.IsTrue(IsPropertyValid("CommittedDate"));
		}

    }
}