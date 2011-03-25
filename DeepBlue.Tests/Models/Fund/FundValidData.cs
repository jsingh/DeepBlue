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
        public void create_a_new_fund_with_fund_name_passes() {
            Assert.IsTrue(IsPropertyValid("FundName"));
        }

        [Test]
        public void create_a_new_fund_with_valid_taxid_passes() {
            Assert.IsTrue(IsPropertyValid("TaxID"));
        }

		[Test]
		public void create_a_new_fund_with_valid_inceptiondate_passes() {
			Assert.IsTrue(IsPropertyValid("InceptionDate"));
		}
    }
}