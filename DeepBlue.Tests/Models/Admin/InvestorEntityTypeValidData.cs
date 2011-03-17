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
    public class InvestorEntityTypeValidDataTest : InvestorEntityTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultInvestorEntityType, true);
			this.ServiceErrors = DefaultInvestorEntityType.Save();
        }

		[Test]
		public void create_a_new_investorentitytype_with_investor_name_passes() {
			Assert.IsTrue(IsPropertyValid("InvestorEntityTypeName"));
		}
    }
}