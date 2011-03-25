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
    public class InvestorTypeInvalidDataTest :InvestorTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultInvestorType, false);
			this.ServiceErrors = DefaultInvestorType.Save();
        }

		[Test]
		public void create_a_new_investortype_without_investortype_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("InvestorTypeName"));
		}

		[Test]
		public void create_a_new_investortype_without_too_long_investortype_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("InvestorTypeName"));
		}
    }
}