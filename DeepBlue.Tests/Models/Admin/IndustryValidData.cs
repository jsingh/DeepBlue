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
    public class IndustryValidDataTest : IndustryTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultIndustry, true);
			this.ServiceErrors = DefaultIndustry.Save();
        }

		[Test]
		public void create_a_new_industry_with_industry_passes() {
			Assert.IsTrue(IsPropertyValid("Industry1"));
		}

    }
}