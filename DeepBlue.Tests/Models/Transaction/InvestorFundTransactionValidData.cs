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
	public class InvestorFundTransactionValidData : InvestorFundTransactionTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultInvestorFundTransaction, true);
			this.ServiceErrors = DefaultInvestorFundTransaction.Save();
        }

		[Test]
		public void create_a_new_investor_fund_transaction_with_valid_fundclosingid_throws_error() {
			Assert.IsTrue(IsPropertyValid("FundClosingID"));
		}

		[Test]
		public void create_a_new_investor_fund_transaction_with_created_by_throws_error() {
			Assert.IsTrue(IsPropertyValid("CreatedBy"));
		}

		[Test]
		public void create_a_new_investor_fund_transaction_with_created_date_throws_error() {
			Assert.IsTrue(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_investor_fund_transaction_with_other_investor_id_throws_error() {
			Assert.IsTrue(IsPropertyValid("OtherInvestorID"));
		}

		[Test]
		public void create_a_new_investor_fund_transaction_with_transaction_type_id_throws_error() {
			Assert.IsTrue(IsPropertyValid("TransactionTypeID"));
		}

		[Test]
		public void create_a_new_investor_fund_transaction_with_amount_throws_error() {
			Assert.IsTrue(IsPropertyValid("Amount"));
		}
    }
}