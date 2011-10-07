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
    public class UserInvalidDataTest : UserTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultUser,false);
            this.ServiceErrors = DefaultUser.Save();
        }

		[Test]
		public void create_a_new_user_without_user_username_throws_error() {
			Assert.IsFalse(IsPropertyValid("UserName"));
		}

		[Test]
		public void create_a_new_user_without_too_long_user_username_throws_error() {
			Assert.IsFalse(IsPropertyValid("UserName"));
		}
        [Test]
        public void create_a_new_user_without_user_firstname_throws_error()
        {
            Assert.IsFalse(IsPropertyValid("FirstName"));
        }

        [Test]
        public void create_a_new_user_without_too_long_user_firstname_throws_error()
        {
            Assert.IsFalse(IsPropertyValid("FirstName"));
        }

        [Test]
        public void create_a_new_user_without_user_lastname_throws_error()
        {
            Assert.IsFalse(IsPropertyValid("LastName"));
        }

        [Test]
        public void create_a_new_user_without_too_long_user_lastname_throws_error()
        {
            Assert.IsFalse(IsPropertyValid("LastName"));
        }

        [Test]
        public void create_a_new_user_without_user_password_throws_error()
        {
            Assert.IsFalse(IsPropertyValid("Password"));
        }

        [Test]
        public void create_a_new_user_without_too_long_user_password_throws_error()
        {
            Assert.IsFalse(IsPropertyValid("Password"));
        }

        [Test]
        public void create_a_new_user_without_user_email_throws_error()
        {
            Assert.IsFalse(IsPropertyValid("Email"));
        }

        [Test]
        public void create_a_new_user_without_too_long_user_email_throws_error()
        {
            Assert.IsFalse(IsPropertyValid("Email"));
        }

    }
}