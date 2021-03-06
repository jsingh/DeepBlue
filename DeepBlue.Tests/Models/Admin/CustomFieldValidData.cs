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
    public class CustomFieldValidDataTest : CustomFieldTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultCustomField, true);
			this.ServiceErrors = DefaultCustomField.Save();
        }

		[Test]
		public void create_a_new_customfield_with_customfieldtext_name_passes() {
			Assert.IsTrue(IsPropertyValid("CustomFieldText"));
		}

		[Test]
		public void create_a_new_customfield_with_moduleid_passes() {
			Assert.IsTrue(IsPropertyValid("ModuleId"));
		}

		[Test]
		public void create_a_new_customfield_with_datatypeid_passes() {
			Assert.IsTrue(IsPropertyValid("DataTypeID"));
		}
    }
}