﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Admin {
    public class FundExpenseTypeValidDataTest : FundExpenseTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultFundExpenseType, true);
            this.ServiceErrors = DefaultFundExpenseType.Save();
        }

		[Test]
		public void create_a_new_fundexpensetype_with_fundexpensetype_name_passes() {
			Assert.IsTrue(IsPropertyValid("Name"));
		}
    }
}