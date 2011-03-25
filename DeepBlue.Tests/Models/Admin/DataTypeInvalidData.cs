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
    public class DataTypeInvalidDataTest :DataTypeTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultDataType, false);
			this.ServiceErrors = DefaultDataType.Save();
        }

		[Test]
		public void create_a_new_datatype_without_datatype_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("DataTypeName"));
		}

		[Test]
		public void create_a_new_datatype_without_too_long_datatype_name_throws_error() {
			Assert.IsFalse(IsPropertyValid("DataTypeName"));
		}
    }
}