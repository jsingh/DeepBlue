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
    public class IndustryInvalidDataTest :IndustryTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultIndustry, false);
			this.ServiceErrors = DefaultIndustry.Save();
        }

		[Test]
		public void create_a_new_industry_without_industry_throws_error() {
			Assert.IsFalse(IsPropertyValid("Industry1"));
		}

		[Test]
		public void create_a_new_industry_without_too_long_industry_throws_error() {
			Assert.IsFalse(IsPropertyValid("Industry1"));
		}

    }
}