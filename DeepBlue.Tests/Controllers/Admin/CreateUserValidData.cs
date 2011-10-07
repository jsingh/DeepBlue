using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Admin;

namespace DeepBlue.Tests.Controllers.Admin {
    public class CreateUserValidData : UserBase {
        private ModelStateDictionary ModelState {
            get {
                return base.ViewResult.ViewData.ModelState;
            }
        }

        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
            // Test if the SaveUser call fails
            MockAdminRepository.Setup(x => x.SaveUser(It.IsAny<DeepBlue.Models.Entity.USER>())).Returns(new List<Helpers.ErrorInfo>());
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
            base.ActionResult = base.DefaultController.UpdateUser(GetValidformCollection());
        }

		#region Tests where form collection doesnt have the required values. Tests for DataAnnotations
		private bool test_posted_value(string parameterName) {
			SetFormCollection();
			return IsValid(parameterName);
		}

		/// <summary>
		/// After the TryUpdateModel tries to update the model, see if the error counts is the same as the DataAnnotations ValidationAttribute count
		/// on the property
		/// </summary>
		/// <param name="parameterName"></param>
		/// <param name="errorCount"></param>
		/// <returns></returns>
		private bool test_error_count(string parameterName, int errorCount) {
			SetFormCollection();
			int errors = 0;
			IsValid(parameterName, out errors);
			return errorCount == errors;
		}

		[Test]
		public void valid_user_username_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("UserName"));
		}

		[Test]
		public void valid_user_username_sets_1_error() {
			Assert.IsTrue(test_error_count("UserName", 0));
		}

        [Test]
        public void valid_user_firstname_sets_model_error_on_model_state()
        {
            Assert.IsTrue(test_posted_value("FirstName"));
        }

        [Test]
        public void valid_user_firstname_sets_1_error()
        {
            Assert.IsTrue(test_error_count("FirstName", 0));
        }

        [Test]
        public void valid_user_lastname_sets_model_error_on_model_state()
        {
            Assert.IsTrue(test_posted_value("LastName"));
        }

        [Test]
        public void valid_user_lastname_sets_1_error()
        {
            Assert.IsTrue(test_error_count("LastName", 0));
        }

        [Test]
        public void valid_user_password_sets_model_error_on_model_state()
        {
            Assert.IsTrue(test_posted_value("Password"));
        }

        [Test]
        public void valid_user_password_sets_1_error()
        {
            Assert.IsTrue(test_error_count("Password", 0));
        }

        [Test]
        public void valid_user_email_sets_model_error_on_model_state()
        {
            Assert.IsTrue(test_posted_value("Email"));
        }

        [Test]
        public void valid_user_email_sets_1_error()
        {
            Assert.IsTrue(test_error_count("Email", 0));
        }

		[Test]
		public void valid_user_name_results_in_valid_modelstate() {
			SetFormCollection();
			Assert.IsTrue(base.DefaultController.ModelState.IsValid);
		}

		#endregion

        #region Tests after model state is invalid
        

        [Test]
        public void returns_back_to_new_view_if_saving_user_failed() {
            SetFormCollection();
			Assert.IsNotNull(Model);
        }

        #endregion
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
			formCollection.Add("UserName", "n/a");
            formCollection.Add("FirstName", "n/a");
            formCollection.Add("LastName", "n/a");
            formCollection.Add("Password", "n/a");
            formCollection.Add("Email", "n/a");
            return formCollection;
        }
    }
}
