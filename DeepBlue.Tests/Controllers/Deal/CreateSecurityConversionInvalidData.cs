using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Deal;
using DeepBlue.Models.Admin;

namespace DeepBlue.Tests.Controllers.Deal {
	public class CreateSecurityConversionInvalidData : CreateSecurityConversion {
		private ResultModel ResultModel {
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
		}

		private void SetFormCollection() {
			FormCollection invalidFormCollection = GetInvalidformCollection();
			base.DefaultController.ValueProvider = SetupValueProvider(invalidFormCollection);
			base.ActionResult = base.DefaultController.CreateConversionActivity(invalidFormCollection);
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
		public void invalid_securityconversion_oldsecurityid_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("OldSecurityId"));
		}

		[Test]
		public void invalid_securityconversion_oldsecurityid_sets_1_error() {
			Assert.IsTrue(test_error_count("OldSecurityId", 1));
		}

		[Test]
		public void invalid_securityconversion_oldsecuritytypeid_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("OldSecurityTypeId"));
		}

		[Test]
		public void invalid_securityconversion_oldsecuritytypeid_sets_1_error() {
			Assert.IsTrue(test_error_count("OldSecurityTypeId", 1));
		}

		[Test]
		public void invalid_securityconversion_newsecurityid_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("NewSecurityId"));
		}

		[Test]
		public void invalid_securityconversion_newsecurityid_sets_1_error() {
			Assert.IsTrue(test_error_count("NewSecurityId", 1));
		}

		[Test]
		public void invalid_securityconversion_newsecuritytypeid_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("NewSecurityTypeId"));
		}

		[Test]
		public void invalid_securityconversion_newsecuritytypeid_sets_1_error() {
			Assert.IsTrue(test_error_count("NewSecurityTypeId", 1));
		}
				
		[Test]
		public void invalid_securityconversion_conversiondate_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("ConversionDate"));
		}

		[Test]
		public void invalid_securityconversion_conversiondate_sets_1_error() {
			Assert.IsTrue(test_error_count("ConversionDate", 1));
		}

		[Test]
		public void invalid_securityconversion_activitytypeid_sets_1_error() {
			Assert.IsTrue(test_error_count("ActivityTypeId", 1));
		}

		[Test]
		public void invalid_securityconversion_splitfactor_sets_1_error() {
			Assert.IsTrue(test_error_count("SplitFactor", 1));
		}
		 
		[Test]
		public void invalid_fund_results_in_invalid_modelstate() {
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
		public void model_state_invalid_redirects_to_result_view() {
			SetModelInvalid();
			Assert.IsNotNull(ResultModel);
		}

		#endregion


		private FormCollection GetInvalidformCollection() {
			FormCollection formCollection = new FormCollection();
			formCollection.Add("OldSecurityId", string.Empty);
			formCollection.Add("OldSecurityTypeId", string.Empty);
			formCollection.Add("NewSecurityId", string.Empty);
			formCollection.Add("NewSecurityTypeId", string.Empty);
			formCollection.Add("ConversionDate", string.Empty);
			formCollection.Add("ActivityTypeId", string.Empty);
			formCollection.Add("SplitFactor", string.Empty);
			return formCollection;
		}
	}
}
