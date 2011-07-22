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
    public class DealUnderlyingFundValidDataTest : DealUnderlyingFundTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultDealUnderlyingFund, true);
			this.ServiceErrors = DefaultDealUnderlyingFund.Save();
        }

        [Test]
		public void create_a_new_dealunderlyingfund_with_fundid_passes() {
			Assert.IsTrue(IsPropertyValid("UnderlyingFundID"));
        }

        [Test]
		public void create_a_new_dealunderlyingfund_with_dealid_passes() {
			Assert.IsTrue(IsPropertyValid("DealID"));
        }

		[Test]
		public void create_a_new_dealunderlyingfund_with_recorddate_passes() {
			Assert.IsTrue(IsPropertyValid("RecordDate"));
		}
    }
}