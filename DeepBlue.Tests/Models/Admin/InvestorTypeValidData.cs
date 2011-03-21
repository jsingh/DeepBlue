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
    public class InvestorTypeValidDataTest : InvestorTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultInvestorType, true);
			this.ServiceErrors = DefaultInvestorType.Save();
        }

		[Test]
		public void create_a_new_investortype_with_investor_name_passes() {
			Assert.IsTrue(IsPropertyValid("InvestorTypeName"));
		}
    }
}