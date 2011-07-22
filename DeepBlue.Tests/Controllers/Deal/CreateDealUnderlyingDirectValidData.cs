using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Deal;

namespace DeepBlue.Tests.Controllers.Deal {
	public class CreateDealUnderlyingDirectValidData : CreateDealUnderlyingDirect {
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
			base.ActionResult = base.DefaultController.CreateDealUnderlyingDirect(validFormCollection);
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
		public void valid_DealUnderlyingDirect_issuerId_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("IssuerId"));
		}

		[Test]
		public void valid_DealUnderlyingDirect_issuerId_sets_0_error() {
			Assert.IsTrue(test_error_count("IssuerId", 0));
		}
		
		[Test]
		public void valid_DealUnderlyingDirect_SecurityTypeId_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("SecurityTypeId"));
		}

		[Test]
		public void valid_DealUnderlyingDirect_SecurityTypeId_sets_0_error() {
			Assert.IsTrue(test_error_count("SecurityTypeId", 0));
		}

		[Test]
		public void valid_DealUnderlyingDirect_SecurityId_sets_0_error() {
			Assert.IsTrue(test_error_count("SecurityId", 0));
		}

		[Test]
		public void valid_DealUnderlyingDirect_SecurityId_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("SecurityId"));
		}

		[Test]
		public void valid_DealUnderlyingDirect_RecordDate_sets_0_error() {
			Assert.IsTrue(test_error_count("RecordDate", 0));
		}

		[Test]
		public void valid_DealUnderlyingDirect_RecordDate_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("RecordDate"));
		}

		[Test]
		public void valid_DealUnderlyingDirect_FMV_sets_0_error() {
			Assert.IsTrue(test_error_count("FMV", 0));
		}

		[Test]
		public void valid_DealUnderlyingDirect_FMV_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("FMV"));
		}

		[Test]
		public void valid_DealUnderlyingDirect_Percent_sets_0_error() {
			Assert.IsTrue(test_error_count("Percent", 0));
		}

		[Test]
		public void valid_DealUnderlyingDirect_Percent_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("Percent"));
		}

		[Test]
		public void valid_DealUnderlyingDirect_NumberOfShares_sets_0_error() {
			Assert.IsTrue(test_error_count("NumberOfShares", 0));
		}

		[Test]
		public void valid_DealUnderlyingDirect_NumberOfShares_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("NumberOfShares"));
		}

		[Test]
		public void valid_DealUnderlyingDirect_PurchasePrice_sets_0_error() {
			Assert.IsTrue(test_error_count("PurchasePrice", 0));
		}

		[Test]
		public void valid_DealUnderlyingDirect_PurchasePrice_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("PurchasePrice"));
		}
		 
		[Test]
		public void valid_DealUnderlyingDirect_DealId_sets_0_error() {
			Assert.IsTrue(test_error_count("DealId", 0));
		}

		[Test]
		public void valid_DealUnderlyingDirect_DealId_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("DealId"));
		}

		[Test]
		public void valid_DealUnderlyingDirect_FundId_sets_0_error() {
			Assert.IsTrue(test_error_count("FundId", 0));
		}

		[Test]
		public void valid_DealUnderlyingDirect_FundId_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("FundId"));
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
			formCollection.Add("IssuerId", "1");
			formCollection.Add("SecurityTypeId", "1");
			formCollection.Add("SecurityId", "1");
			formCollection.Add("DealId", "1");
			formCollection.Add("FundId", "1");
			formCollection.Add("RecordDate", DateTime.MaxValue.ToString());
			formCollection.Add("FMV", "1");
			formCollection.Add("Percent", "1");
			formCollection.Add("NumberOfShares", "1");
			formCollection.Add("PurchasePrice", "1");
			return formCollection;
		}
	}
}
