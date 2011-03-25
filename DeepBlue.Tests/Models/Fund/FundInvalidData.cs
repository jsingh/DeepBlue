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
    public class FundInvalidDataTest : FundTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
            Create_Data(DefaultFund, false);
			this.ServiceErrors = DefaultFund.Save();
        }

        [Test]
        public void create_a_new_fund_without_fund_name_throws_error() {
            Assert.IsFalse(IsPropertyValid("FundName"));
        }

        [Test]
        public void create_a_new_fund_with_too_long_fund_name_throws_error() {
            Assert.IsFalse(IsPropertyValid("FundName"));
        }

        [Test]
        public void create_a_new_fund_without_valid_taxid_throws_error() {
            Assert.IsTrue(IsPropertyValid("TaxID"));
        }
    }
}