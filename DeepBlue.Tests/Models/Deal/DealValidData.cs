using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Deal {
    public class DealValidDataTest : DealTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
            Create_Data(DefaultDeal, true);
            this.ServiceErrors = DefaultDeal.Save();
        }

        [Test]
        public void create_a_new_deal_with_fundid_passes() {
			Assert.IsTrue(IsPropertyValid("FundID"));
        }

        [Test]
        public void create_a_new_deal_with_dealnumber_passes() {
			Assert.IsTrue(IsPropertyValid("DealNumber"));
        }

		[Test]
		public void create_a_new_deal_with_purchasetypeid_passes() {
			Assert.IsTrue(IsPropertyValid("PurchaseTypeID"));
		}

		[Test]
		public void create_a_new_deal_with_dealname_passes() {
			Assert.IsTrue(IsPropertyValid("DealName"));
		}

    }
}