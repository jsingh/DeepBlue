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
	public class InvestorFundTransactionInvalidData : InvestorFundTransactionTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultInvestorFundTransaction, false);
			this.ServiceErrors = DefaultInvestorFundTransaction.Save();
        }

        [Test]
        public void create_a_new_investor_fund_transaction_without_valid_fundclosingid_throws_error() {
			Assert.IsFalse(IsPropertyValid("FundClosingID"));
        }

        [Test]
        public void create_a_new_investor_fund_transaction_without_created_by_throws_error() {
			Assert.IsFalse(IsPropertyValid("CreatedBy"));
        }

		[Test]
		public void create_a_new_investor_fund_transaction_without_created_date_throws_error() {
			Assert.IsFalse(IsPropertyValid("CreatedDate"));
		}

		[Test]
		public void create_a_new_investor_fund_transaction_without_other_investor_id_throws_error() {
			Assert.IsFalse(IsPropertyValid("OtherInvestorID"));
		}

		[Test]
		public void create_a_new_investor_fund_transaction_without_transaction_type_id_throws_error() {
			Assert.IsFalse(IsPropertyValid("TransactionTypeID"));
		}

		[Test]
		public void create_a_new_investor_fund_transaction_without_amount_throws_error() {
			Assert.IsFalse(IsPropertyValid("Amount"));
		}
    }
}