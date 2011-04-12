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
    public class CustomFieldInvalidDataTest :CustomFieldTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultCustomField, false);
			this.ServiceErrors = DefaultCustomField.Save();
        }

		[Test]
		public void create_a_new_customfield_without_customfieldtext_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("CustomFieldText"));
		}

		[Test]
		public void create_a_new_customfield_without_too_long_customfieldtext_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("CustomFieldText"));
		}

		[Test]
		public void create_a_new_customfield_without_moduleid_throws_error() {
			Assert.IsTrue(IsPropertyValid("ModuleId"));
		}

		[Test]
		public void create_a_new_customfield_without_datatypeid_throws_error() {
			Assert.IsFalse(IsPropertyValid("DataTypeID"));
		}
    }
}