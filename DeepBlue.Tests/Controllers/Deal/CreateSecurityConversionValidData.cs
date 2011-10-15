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
	public class CreateSecurityConversionValidData : CreateSecurityConversion {
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
			FormCollection validFormCollection = GetValidformCollection();
			base.DefaultController.ValueProvider = SetupValueProvider(validFormCollection);
			base.ActionResult = base.DefaultController.CreateConversionActivity(validFormCollection);
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
		public void valid_securityconversion_oldsecurityid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("OldSecurityId"));
		}

		[Test]
		public void valid_securityconversion_oldsecurityid_sets_0_error() {
			Assert.IsTrue(test_error_count("OldSecurityId", 0));
		}

		[Test]
		public void valid_securityconversion_oldsecuritytypeid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("OldSecurityTypeId"));
		}

		[Test]
		public void valid_securityconversion_oldsecuritytypeid_sets_0_error() {
			Assert.IsTrue(test_error_count("OldSecurityTypeId", 0));
		}

		[Test]
		public void valid_securityconversion_newsecurityid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("NewSecurityId"));
		}

		[Test]
		public void valid_securityconversion_newsecurityid_sets_0_error() {
			Assert.IsTrue(test_error_count("NewSecurityId", 0));
		}

		[Test]
		public void valid_securityconversion_newsecuritytypeid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("NewSecurityTypeId"));
		}

		[Test]
		public void valid_securityconversion_newsecuritytypeid_sets_0_error() {
			Assert.IsTrue(test_error_count("NewSecurityTypeId", 0));
		}

		[Test]
		public void valid_securityconversion_conversiondate_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("ConversionDate"));
		}

		[Test]
		public void valid_securityconversion_conversiondate_sets_0_error() {
			Assert.IsTrue(test_error_count("ConversionDate", 0));
		}

		[Test]
		public void valid_securityconversion_activitytypeid_sets_0_error() {
			Assert.IsTrue(test_error_count("ActivityTypeId", 0));
		}

		[Test]
		public void valid_securityconversion_splitfactor_sets_0_error() {
			Assert.IsTrue(test_error_count("SplitFactor", 0));
		}

		[Test]
		public void valid_fund_results_in_valid_modelstate() {
			SetFormCollection();
			Assert.IsTrue(base.DefaultController.ModelState.IsValid);
		}

		#endregion

		#region Tests after model state is valid

		private void SetModelValid() {
			base.DefaultController.ModelState.AddModelError(string.Empty, string.Empty);
			SetFormCollection();
		}

		[Test]
		public void model_state_valid_redirects_to_result_view() {
			SetModelValid();
			Assert.IsNotNull(ResultModel);
		}

		#endregion


		private FormCollection GetValidformCollection() {
			FormCollection formCollection = new FormCollection();
			formCollection.Add("OldSecurityId", "1");
			formCollection.Add("OldSecurityTypeId", "1");
			formCollection.Add("NewSecurityId", "1");
			formCollection.Add("NewSecurityTypeId", "1");
			formCollection.Add("ConversionDate", DateTime.MaxValue.ToString());
			formCollection.Add("ActivityTypeId", "1");
			formCollection.Add("SplitFactor", "1");
			return formCollection;
		}
	}
}
