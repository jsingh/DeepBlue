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
	public class UnderlyingFundTypeInvalidDataTest : UnderlyingFundTypeTest {

		[SetUp]
		public override void Setup() {
			base.Setup();
			Create_Data(DefaultUnderlyingFundType, false);
			this.ServiceErrors = DefaultUnderlyingFundType.Save();
		}

		[Test]
		public void create_a_new_underlyingfundtype_without_underlyingfund_throws_error() {
			Assert.IsFalse(IsPropertyValid("Name"));
		}

		[Test]
		public void create_a_new_underlyingfundtype_without_too_long_underlyingfund_throws_error() {
			Assert.IsFalse(IsPropertyValid("Name"));
		}

	}
}