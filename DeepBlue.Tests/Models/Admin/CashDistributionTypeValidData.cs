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
    public class CashDistributionTypeValidDataTest : CashDistributionTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultCashDistributionType, true);
			this.ServiceErrors = DefaultCashDistributionType.Save();
        }

		[Test]
		public void create_a_new_cashdistributiontype_with_cashdistributiontype_passes() {
			Assert.IsTrue(IsPropertyValid("Name"));
		}

    }
}