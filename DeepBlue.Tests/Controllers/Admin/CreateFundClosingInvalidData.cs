using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Admin;

namespace DeepBlue.Tests.Controllers.Admin {
    public class CreateFundClosingInvalidData : EditFundClosing {
        private ModelStateDictionary ModelState {
            get {
                return base.ViewResult.ViewData.ModelState;
            }
        }

        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(new FormCollection());
			base.ActionResult = base.DefaultController.UpdateFundClosing(GetInvalidformCollection());
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
        public void invalid_fundclosing_name_sets_model_error_on_model_state() {
            Assert.IsFalse(test_posted_value("Name"));
        }

        [Test]
        public void invalid_fundclosing_name_sets_1_error() {
			Assert.IsTrue(test_error_count("Name", 1));
        }

		[Test]
		public void invalid_fundclosing_fundid_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("FundID"));
		}

		[Test]
		public void invalid_fundclosing_fundid_sets_1_error() {
			Assert.IsTrue(test_error_count("FundID", 1));
		}


		[Test]
		public void invalid_fundclosing_fundclosingdate_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("FundClosingDate"));
		}

		[Test]
		public void invalid_fundclosing_fundclosingdate_sets_1_error() {
			Assert.IsTrue(test_error_count("FundClosingDate", 1));
		}

        [Test]
        public void invalid_fundclosing_name_results_in_invalid_modelstate() {
            SetFormCollection();
            Assert.IsFalse(base.DefaultController.ModelState.IsValid);
        }
      
        #endregion

        #region Tests after model state is invalid
        private void SetModelInvalid() {
            base.DefaultController.ModelState.AddModelError(string.Empty, string.Empty);
            SetFormCollection();
        }

       
        #endregion


        private FormCollection GetInvalidformCollection() {
            FormCollection formCollection = new FormCollection();
			formCollection.Add("Name", string.Empty);
			formCollection.Add("FundID", string.Empty);
			formCollection.Add("FundClosingDate", string.Empty);			
            return formCollection;
        }
    }
}
