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
    public class UserValidDataTest : UserTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultUser, true);
            this.ServiceErrors = DefaultUser.Save();
        }

		[Test]
		public void create_a_new_user_with_user_username_passes() {
			Assert.IsTrue(IsPropertyValid("UserName"));
		}
        [Test]
        public void create_a_new_user_with_user_firstname_passes()
        {
            Assert.IsTrue(IsPropertyValid("FirstName"));
        }
        [Test]
        public void create_a_new_user_with_user_lastname_passes()
        {
            Assert.IsTrue(IsPropertyValid("LastName"));
        }
        [Test]
        public void create_a_new_user_with_user_password_passes()
        {
            Assert.IsTrue(IsPropertyValid("Password"));
        }
        
    }
}