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
    public class GeographyInvalidDataTest :GeographyTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultGeography, false);
			this.ServiceErrors = DefaultGeography.Save();
        }

		[Test]
		public void create_a_new_geography_without_geography_throws_error() {
			Assert.IsFalse(IsPropertyValid("Geography1"));
		}

		[Test]
		public void create_a_new_geography_without_too_long_geography_throws_error() {
			Assert.IsFalse(IsPropertyValid("Geography1"));
		}

    }
}