using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Deal;

namespace DeepBlue.Tests.Controllers.Deal {
	public class CreateDealUnderlyingFundValidData : CreateDealUnderlyingFund {
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
			base.ActionResult = base.DefaultController.CreateDealUnderlyingFund(validFormCollection);
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
		public void valid_DealUnderlying_fundid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("UnderlyingFundId"));
		}

		[Test]
		public void valid_DealUnderlying_fundid_sets_0_error() {
			Assert.IsTrue(test_error_count("UnderlyingFundId", 0));
		}

		[Test]
		public void valid_DealUnderlying_dealid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("DealId"));
		}

		[Test]
		public void valid_DealUnderlying_dealid_sets_0_error() {
			Assert.IsTrue(test_error_count("DealId", 0));
		}

		[Test]
		public void valid_DealUnderlying_FundId_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("FundId"));
		}

		[Test]
		public void valid_DealUnderlying_FundId_sets_0_error() {
			Assert.IsTrue(test_error_count("FundId", 0));
		}

		[Test]
		public void valid_DealUnderlying_FundNAV_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("FundNAV"));
		}

		[Test]
		public void valid_DealUnderlying_FundNAV_sets_0_error() {
			Assert.IsTrue(test_error_count("FundNAV", 0));
		}

		[Test]
		public void valid_DealUnderlying_Percent_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("Percent"));
		}

		[Test]
		public void valid_DealUnderlying_Percent_sets_0_error() {
			Assert.IsTrue(test_error_count("Percent", 0));
		}

		[Test]
		public void valid_DealUnderlying_CommittedAmount_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("CommittedAmount"));
		}

		[Test]
		public void valid_DealUnderlying_CommittedAmount_sets_0_error() {
			Assert.IsTrue(test_error_count("CommittedAmount", 0));
		}

		[Test]
		public void valid_DealUnderlying_GrossPurchasePrice_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("GrossPurchasePrice"));
		}

		[Test]
		public void valid_DealUnderlying_GrossPurchasePrice_sets_0_error() {
			Assert.IsTrue(test_error_count("GrossPurchasePrice", 0));
		}

		[Test]
		public void valid_DealUnderlying_ReassignedGPP_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("ReassignedGPP"));
		}

		[Test]
		public void valid_DealUnderlying_ReassignedGPP_sets_0_error() {
			Assert.IsTrue(test_error_count("ReassignedGPP", 0));
		}

		[Test]
		public void valid_DealUnderlying_recorddate_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("RecordDate"));
		}

		[Test]
		public void valid_DealUnderlying_recorddate_sets_0_error() {
			Assert.IsTrue(test_error_count("RecordDate", 0));
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
			formCollection.Add("FundId", "1");
			formCollection.Add("UnderlyingFundId", "1");
			formCollection.Add("RecordDate", DateTime.MaxValue.ToString());
			formCollection.Add("FundNAV", "1");
			formCollection.Add("Percent", "1");
			formCollection.Add("CommittedAmount", "1");
			formCollection.Add("GrossPurchasePrice", "1");
			formCollection.Add("ReassignedGPP", "1");
			return formCollection;
		}
	}
}
