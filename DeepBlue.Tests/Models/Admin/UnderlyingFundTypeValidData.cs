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
	public class UnderlyingFundTypeValidDataTest : UnderlyingFundTypeTest {

		[SetUp]
		public override void Setup() {
			base.Setup();
			Create_Data(DefaultUnderlyingFundType, true);
			this.ServiceErrors = DefaultUnderlyingFundType.Save();
		}

		[Test]
		public void create_a_new_underlyingfundtype_with_underlyingfundtype_passes() {
			Assert.IsTrue(IsPropertyValid("Name"));
		}

	}
}