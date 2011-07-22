using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Deal;

namespace DeepBlue.Tests.Controllers.Deal {
	public class CreateUnderlyingFundPostRecordCapitalCallInvalidData : CreateUnderlyingFundPostRecordCapitalCall {
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
			base.ActionResult = base.DefaultController.CreateUnderlyingFundPostRecordCapitalCall(invalidFormCollection);
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
		public void invalid_UnderlyingFundPostRecordCapitalCall_UnderlyingFundId_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("UnderlyingFundId"));
		}

		[Test]
		public void invalid_UnderlyingFundPostRecordCapitalCall_UnderlyingFundId_sets_1_error() {
			Assert.IsTrue(test_error_count("UnderlyingFundId", 1));
		}

		[Test]
		public void invalid_UnderlyingFundPostRecordCapitalCall_FundId_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("FundId"));
		}

		[Test]
		public void invalid_UnderlyingFundPostRecordCapitalCall_FundId_sets_1_error() {
			Assert.IsTrue(test_error_count("FundId", 1));
		}

		[Test]
		public void invalid_UnderlyingFundPostRecordCapitalCall_DealId_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("DealId"));
		}

		[Test]
		public void invalid_UnderlyingFundPostRecordCapitalCall_DealId_sets_1_error() {
			Assert.IsTrue(test_error_count("DealId", 1));
		}

		[Test]
		public void invalid_UnderlyingFundPostRecordCapitalCall_Amount_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("Amount"));
		}

		[Test]
		public void invalid_UnderlyingFundPostRecordCapitalCall_Amount_sets_1_error() {
			Assert.IsTrue(test_error_count("Amount", 1));
		}

		[Test]
		public void invalid_UnderlyingFundPostRecordCapitalCall_CapitalCallDate_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("CapitalCallDate"));
		}

		[Test]
		public void invalid_UnderlyingFundPostRecordCapitalCall_CapitalCallDate_sets_1_error() {
			Assert.IsTrue(test_error_count("CapitalCallDate", 1));
		}
		
		[Test]
		public void invalid_Fund_results_in_invalid_modelstate() {
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
			formCollection.Add("FundId", string.Empty);
			formCollection.Add("DealId", string.Empty);
			formCollection.Add("UnderlyingFundId", string.Empty);
			formCollection.Add("Amount", string.Empty);
			formCollection.Add("CapitalCallDate", string.Empty);
			formCollection.Add("TotalRows","1");
			return formCollection;
		}
	}
}
