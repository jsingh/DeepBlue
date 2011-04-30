using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Admin {
    public class DealClosingCostTypeInvalidDataTest :DealClosingCostTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultDealClosingCostType, false);
			this.ServiceErrors = DefaultDealClosingCostType.Save();
        }

		[Test]
		public void create_a_new_dealclosingcosttype_without_dealclosingcosttype_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("Name"));
		}

		[Test]
		public void create_a_new_dealclosingcosttype_without_too_long_dealclosingcosttype_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("Name"));
		}
    }
}