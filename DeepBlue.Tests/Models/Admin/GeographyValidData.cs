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
    public class GeographyValidDataTest : GeographyTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultGeography, true);
			this.ServiceErrors = DefaultGeography.Save();
        }

		[Test]
		public void create_a_new_geography_with_geography_passes() {
			Assert.IsTrue(IsPropertyValid("Geography1"));
		}

    }
}