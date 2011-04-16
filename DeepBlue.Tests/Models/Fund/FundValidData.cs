using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Fund {
    public class FundValidDataTest : FundTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
            Create_Data(DefaultFund, true);
            this.ServiceErrors = DefaultFund.Save();
        }

		[Test]
		public void create_a_new_fund_with_entityid_passes() {
			Assert.IsFalse(IsPropertyValid("EntityID"));
		}

        [Test]
        public void create_a_new_fund_with_fund_name_passes() {
            Assert.IsTrue(IsPropertyValid("FundName"));
        }

        [Test]
        public void create_a_new_fund_with_taxid_passes() {
			Assert.IsTrue(IsPropertyValid("TaxId"));
        }

		[Test]
		public void create_a_new_fund_with_inceptiondate_passes() {
			Assert.IsTrue(IsPropertyValid("InceptionDate"));
		}

		[Test]
		public void create_a_new_fund_with_bank_name_passes() {
			Assert.IsTrue(IsPropertyValid("BankName"));
		}

		[Test]
		public void create_a_new_fund_with_account_passes() {
			Assert.IsTrue(IsPropertyValid("Account"));
		}

    }
}