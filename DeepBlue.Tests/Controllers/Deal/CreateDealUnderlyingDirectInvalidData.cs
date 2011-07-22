using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Deal;

namespace DeepBlue.Tests.Controllers.Deal {
	public class CreateDealUnderlyingDirectInvalidData : CreateDealUnderlyingDirect {
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
			base.ActionResult = base.DefaultController.CreateDealUnderlyingDirect(invalidFormCollection);
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
		public void invalid_DealUnderlyingDirect_issuerId_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("IssuerId"));
		}

		[Test]
		public void invalid_DealUnderlyingDirect_issuerId_sets_1_error() {
			Assert.IsTrue(test_error_count("IssuerId", 1));
		}
		
		[Test]
		public void invalid_DealUnderlyingDirect_SecurityTypeId_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("SecurityTypeId"));
		}

		[Test]
		public void invalid_DealUnderlyingDirect_SecurityTypeId_sets_1_error() {
			Assert.IsTrue(test_error_count("SecurityTypeId", 1));
		}

		[Test]
		public void invalid_DealUnderlyingDirect_SecurityId_sets_1_error() {
			Assert.IsTrue(test_error_count("SecurityId", 1));
		}

		[Test]
		public void invalid_DealUnderlyingDirect_SecurityId_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("SecurityId"));
		}

		[Test]
		public void invalid_DealUnderlyingDirect_RecordDate_sets_1_error() {
			Assert.IsTrue(test_error_count("RecordDate", 1));
		}

		[Test]
		public void invalid_DealUnderlyingDirect_RecordDate_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("RecordDate"));
		}

		[Test]
		public void invalid_DealUnderlyingDirect_FMV_sets_1_error() {
			Assert.IsTrue(test_error_count("FMV", 1));
		}

		[Test]
		public void invalid_DealUnderlyingDirect_FMV_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("FMV"));
		}
		 
		[Test]
		public void invalid_DealUnderlyingDirect_NumberOfShares_sets_1_error() {
			Assert.IsTrue(test_error_count("NumberOfShares", 1));
		}

		[Test]
		public void invalid_DealUnderlyingDirect_NumberOfShares_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("NumberOfShares"));
		}

		[Test]
		public void invalid_DealUnderlyingDirect_PurchasePrice_sets_1_error() {
			Assert.IsTrue(test_error_count("PurchasePrice", 1));
		}

		[Test]
		public void invalid_DealUnderlyingDirect_PurchasePrice_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("PurchasePrice"));
		}
		 
		[Test]
		public void invalid_DealUnderlyingDirect_DealId_sets_1_error() {
			Assert.IsTrue(test_error_count("DealId", 1));
		}

		[Test]
		public void invalid_DealUnderlyingDirect_DealId_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("DealId"));
		}

		[Test]
		public void invalid_DealUnderlyingDirect_FundId_sets_1_error() {
			Assert.IsTrue(test_error_count("FundId", 1));
		}

		[Test]
		public void invalid_DealUnderlyingDirect_FundId_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("FundId"));
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
			formCollection.Add("IssuerId", string.Empty);
			formCollection.Add("SecurityTypeId", string.Empty);
			formCollection.Add("SecurityId", string.Empty);
			formCollection.Add("DealId", string.Empty);
			formCollection.Add("FundId", string.Empty);
			formCollection.Add("RecordDate", string.Empty);
			formCollection.Add("FMV", string.Empty);
			formCollection.Add("Percent", "-1");
			formCollection.Add("NumberOfShares", string.Empty);
			formCollection.Add("PurchasePrice", string.Empty);
			return formCollection;
		}
	}
}
