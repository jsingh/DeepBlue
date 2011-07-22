﻿using System;
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
			base.ActionResult = base.DefaultController.Create (GetInvalidformCollection());
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
		public void invalid_Investor_name_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("InvestorName"));
		}

		[Test]
		public void invalid_Investor_name_sets_1_error() {
			Assert.IsTrue(test_error_count("InvestorName", 1));
		}

        [Test]
        public void invalid_Investor_social_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("Social"));
        }

        [Test]
        public void invalid_Investor_social_sets_1_error() {
			Assert.IsTrue(test_error_count("Social", 1));
        }

		[Test]
		public void invalid_Investor_alias_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("Alias"));
		}

		[Test]
		public void invalid_Investor_alias_sets_1_error() {
			Assert.IsTrue(test_error_count("Alias", 1));
		}

		[Test]
		public void invalid_Investor_phone_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("Phone"));
		}

		[Test]
		public void invalid_Investor_phone_sets_1_error() {
			Assert.IsTrue(test_error_count("Phone", 1));
		}

		[Test]
		public void invalid_Investor_email_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("Email"));
		}

		[Test]
		public void invalid_Investor_email_sets_1_error() {
			Assert.IsTrue(test_error_count("Email", 1));
		}

		[Test]
		public void invalid_Investor_address_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("Address1"));
		}

		[Test]
		public void invalid_Investor_address_sets_1_error() {
			Assert.IsTrue(test_error_count("Address1", 1));
		}

		[Test]
		public void invalid_Investor_city_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("City"));
		}

		[Test]
		public void invalid_Investor_city_sets_1_error() {
			Assert.IsTrue(test_error_count("City", 1));
		}

		[Test]
		public void invalid_Investor_zip_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("Zip"));
		}

		[Test]
		public void invalid_Investor_zip_sets_1_error() {
			Assert.IsTrue(test_error_count("Zip", 1));
		}


        [Test]
        public void invalid_Investor_name_results_in_invalid_modelstate() {
            SetFormCollection();
            Assert.IsFalse(base.DefaultController.ModelState.IsValid);
        }
      
        #endregion

        #region Tests after model state is invalid
        private void SetModelInvalid() {
            base.DefaultController.ModelState.AddModelError(string.Empty, string.Empty);
            SetFormCollection();
        }

        [Test]
        public void model_state_invalid_redirects_to_new_view() {
            SetModelInvalid();
            Assert.IsNotNull(Model);
        }

        [Test]
        public void model_state_invalid_redirects_new_view_countries_populated() {
            SetModelInvalid();
            // Make sure it is a redirect
            Assert.IsTrue(Model.SelectList.Countries.Count > 0);
        }

        [Test]
        public void model_state_invalid_redirects_new_view_states_populated() {
            SetModelInvalid();
            // Make sure it is a redirect
            Assert.IsTrue(Model.SelectList.States.Count > 0);
        }

        [Test]
        public void model_state_invalid_redirects_new_view_addresstypes_populated() {
            SetModelInvalid();
            // Make sure it is a redirect
            Assert.IsTrue(Model.SelectList.AddressTypes.Count > 0);
        }

        [Test]
        public void model_state_invalid_redirects_new_view_domesticforeigns_populated() {
            SetModelInvalid();
            // Make sure it is a redirect
            Assert.IsTrue(Model.SelectList.DomesticForeigns.Count > 0);
        }

        #endregion
 
        private FormCollection GetInvalidformCollection() {
            FormCollection formCollection = new FormCollection();
            formCollection.Add("InvestorName", string.Empty);
            formCollection.Add("Alias", string.Empty);
            formCollection.Add("Phone", string.Empty);
            formCollection.Add("Email", string.Empty);
            formCollection.Add("Address1", string.Empty);
            formCollection.Add("City", string.Empty);
            formCollection.Add("Zip", string.Empty);
            return formCollection;
        }
    }
}
