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
    public class EquityTypeValidDataTest : EquityTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultEquityType, true);
			this.ServiceErrors = DefaultEquityType.Save();
        }

		[Test]
		public void create_a_new_equitytype_with_equity_passes() {
			Assert.IsTrue(IsPropertyValid("Equity"));
		}

    }
}