using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Admin;

namespace DeepBlue.Tests.Controllers.Admin {
    public class CreateShareClassTypeValidData : ShareClassTypeBase {

		protected ResultModel ResultModel {
			get {
				return base.ViewResult.ViewData.Model as ResultModel;
			}
		}

        private ModelStateDictionary ModelState {
            get {
                return base.ViewResult.ViewData.ModelState;
            }
        }

        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
            // Test if the SaveShareClassType call fails
            MockAdminRepository.Setup(x => x.SaveShareClassType(It.IsAny<DeepBlue.Models.Entity.ShareClassType>())).Returns(new List<Helpers.ErrorInfo>());
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
            base.ActionResult = base.DefaultController.UpdateShareClassType(GetValidformCollection());
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
		public void valid_shareclasstype_shareclass_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("ShareClass"));
		}

		[Test]
		public void valid_shareclasstype_shareclass_sets_1_error() {
			Assert.IsTrue(test_error_count("ShareClass", 0));
		}

		[Test]
		public void valid_shareclasstype_shareclass_results_in_valid_modelstate() {
			SetFormCollection();
			Assert.IsTrue(base.DefaultController.ModelState.IsValid);
		}

		#endregion

        #region Tests after model state is valid
        [Test]
        public void returns_back_to_new_view_if_saving_shareclasstype_failed() {
            SetFormCollection();
			Assert.IsNotNull(ResultModel);
        }

        #endregion
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
			formCollection.Add("ShareClass", "Test");
            return formCollection;
        }
    }
}
