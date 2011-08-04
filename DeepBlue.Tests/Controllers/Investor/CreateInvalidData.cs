using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Investor;

namespace DeepBlue.Tests.Controllers.Investor {
    public class CreateInvalidData : Create {
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
			base.ActionResult = base.DefaultController.Create(GetInvalidformCollection());
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
		public void invalid_investor_name_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("InvestorName"));
		}

		[Test]
		public void invalid_investor_name_sets_1_error() {
			Assert.IsTrue(test_error_count("InvestorName", 1));
		}

        [Test]
        public void invalid_investor_socialsecuritytaxid_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("SocialSecurityTaxId"));
        }

		[Test]
		public void invalid_investor_socialsecuritytaxid_sets_1_error() {
			Assert.IsTrue(test_error_count("SocialSecurityTaxId", 1));
		}

        [Test]
		public void invalid_investor_stateofresidency_sets_1_error() {
			Assert.IsTrue(test_error_count("StateOfResidency", 1));
        }

		[Test]
		public void invalid_investor_stateofresidency_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("StateOfResidency"));
		}

		[Test]
		public void invalid_investor_entitytype_sets_1_error() {
			Assert.IsTrue(test_error_count("EntityType", 1));
		}

		[Test]
		public void invalid_investor_entitytype_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("EntityType"));
		}
		
        [Test]
        public void invalid_investor_name_results_in_invalid_modelstate() {
            SetFormCollection();
            Assert.IsFalse(base.DefaultController.ModelState.IsValid);
        }
      
        #endregion
 
        private FormCollection GetInvalidformCollection() {
            FormCollection formCollection = new FormCollection();
            formCollection.Add("InvestorName", string.Empty);
			formCollection.Add("SocialSecurityTaxId", string.Empty);
			formCollection.Add("StateOfResidency", string.Empty);
			formCollection.Add("EntityType", string.Empty);
            return formCollection;
        }
    }
}
