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
    public class DealUnderlyingFundInvalidDataTest : DealUnderlyingFundTest  {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultDealUnderlyingFund, false);
			this.ServiceErrors = DefaultDealUnderlyingFund.Save();
        }

		[Test]
		public void create_a_new_dealUnderlying_without_fundit_throws_error() {
			Assert.IsFalse(IsPropertyValid("UnderlyingFundID"));
		}

		[Test]
		public void create_a_new_dealUnderlying_without_dealid_throws_error() {
			Assert.IsFalse(IsPropertyValid("DealID"));
		}

		[Test]
		public void create_a_new_dealUnderlying_without_recorddate_throws_error() {
			Assert.IsFalse(IsPropertyValid("RecordDate"));
		}
    }
}