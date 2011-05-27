﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Admin;

namespace DeepBlue.Tests.Controllers.Admin {
    public class CreateFundClosingValidData :EditFundClosing {
        private ModelStateDictionary ModelState {
            get {
                return base.ViewResult.ViewData.ModelState;
            }
        }

        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
            // Test if the SaveFundClosing call fails
            MockAdminRepository.Setup(x => x.SaveFundClosing(It.IsAny<DeepBlue.Models.Entity.FundClosing>())).Returns(new List<Helpers.ErrorInfo>());
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
            base.ActionResult = base.DefaultController.UpdateFundClosing(GetValidformCollection());
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
		public void valid_fundclosing_name_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("Name"));
		}

		[Test]
		public void valid_fundclosing_name_sets_1_error() {
			Assert.IsTrue(test_error_count("Name", 0));
		}

		[Test]
		public void valid_fundclosing_fundid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("FundID"));
		}

		[Test]
		public void valid_fundclosing_fundid_sets_1_error() {
			Assert.IsTrue(test_error_count("FundID", 0));
		}

		[Test]
		public void valid_fundclosing_fundclosingdate_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("FundClosingDate"));
		}

		[Test]
		public void valid_fundclosing_fundclosingdate_sets_1_error() {
			Assert.IsTrue(test_error_count("FundClosingDate", 0));
		}

		[Test]
		public void valid_fundclosing_name_results_in_valid_modelstate() {
			SetFormCollection();
			Assert.IsTrue(base.DefaultController.ModelState.IsValid);
		}

		#endregion

        #region Tests after model state is invalid
        [Test]
        public void returns_back_to_new_view_if_saving_fun_failed() {
            SetFormCollection();
            Assert.IsNull(Model);
        }
        #endregion
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
			formCollection.Add("Name","n/a");
			formCollection.Add("FundID", "1");
			formCollection.Add("FundClosingDate", "1/1/9999");
            return formCollection;
        }
    }
}