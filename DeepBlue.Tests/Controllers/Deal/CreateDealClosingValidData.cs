using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Deal;

namespace DeepBlue.Tests.Controllers.Deal {
	public class CreateDealClosingValidData : CreateDealClosing {
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
			base.ActionResult = base.DefaultController.UpdateDealClosing(validFormCollection);
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
		public void valid_DealClosing_DealId_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("DealId"));
		}

		[Test]
		public void valid_DealClosing_DealId_sets_0_error() {
			Assert.IsTrue(test_error_count("DealId", 0));
		}

		[Test]
		public void valid_DealClosing_FundId_sets_0_error() {
			Assert.IsTrue(test_error_count("FundId", 0));
		}

		[Test]
		public void valid_DealClosing_DealNumber_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("DealNumber"));
		}

		[Test]
		public void valid_DealClosing_DealNumber_sets_0_error() {
			Assert.IsTrue(test_error_count("DealNumber", 0));
		}


		[Test]
		public void valid_DealClosing_CloseDate_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("CloseDate"));
		}

		[Test]
		public void valid_DealClosing_CloseDate_sets_0_error() {
			Assert.IsTrue(test_error_count("CloseDate", 0));
		}
 
		[Test]
		public void valid_Fund_results_in_valid_modelstate() {
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
			formCollection.Add("DealId", "1");
			formCollection.Add("DealNumber", "1");
			formCollection.Add("FundId", "1");
			formCollection.Add("CloseDate", DateTime.MaxValue.ToString());
			return formCollection;
		}
	}
}
