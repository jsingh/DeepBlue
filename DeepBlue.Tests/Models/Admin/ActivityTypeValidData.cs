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
    public class ActivityTypeValidDataTest : ActivityTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultActivityType, true);
			this.ServiceErrors = DefaultActivityType.Save();
        }

		[Test]
		public void create_a_new_ActivityType_with_activitytype_passes() {
			Assert.IsTrue(IsPropertyValid("Name"));
		}

    }
}