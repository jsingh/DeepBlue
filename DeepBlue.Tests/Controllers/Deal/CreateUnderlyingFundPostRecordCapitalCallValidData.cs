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
	public class CreateUnderlyingFundPostRecordCapitalCallValidData : CreateUnderlyingFundPostRecordCapitalCall {
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
			base.ActionResult = base.DefaultController.CreateUnderlyingFundPostRecordCapitalCall(validFormCollection);
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
		public void valid_underlyingfundpostrecordcapitalcall_underlyingfundid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("UnderlyingFundId"));
		}

		[Test]
		public void valid_underlyingfundpostrecordcapitalcall_underlyingfundid_sets_0_error() {
			Assert.IsTrue(test_error_count("UnderlyingFundId", 0));
		}

		[Test]
		public void valid_underlyingfundpostrecordcapitalcall_fundid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("FundId"));
		}

		[Test]
		public void valid_underlyingfundpostrecordcapitalcall_fundid_sets_0_error() {
			Assert.IsTrue(test_error_count("FundId", 0));
		}

		[Test]
		public void valid_underlyingfundpostrecordcapitalcall_dealid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("DealId"));
		}

		[Test]
		public void valid_underlyingfundpostrecordcapitalcall_dealid_sets_0_error() {
			Assert.IsTrue(test_error_count("DealId", 0));
		}

		[Test]
		public void valid_underlyingfundpostrecordcapitalcall_amount_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("Amount"));
		}

		[Test]
		public void valid_underlyingfundpostrecordcapitalcall_amount_sets_0_error() {
			Assert.IsTrue(test_error_count("Amount", 0));
		}

		[Test]
		public void valid_underlyingfundpostrecordcapitalcall_capitalcalldate_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("CapitalCallDate"));
		}

		[Test]
		public void valid_underlyingfundpostrecordcapitalcall_capitalcalldate_sets_0_error() {
			Assert.IsTrue(test_error_count("CapitalCallDate", 0));
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
			formCollection.Add("0_UnderlyingFundCapitalCallLineItemId", "0");
			formCollection.Add("0_FundId", "1");
			formCollection.Add("0_DealId", "1");
			formCollection.Add("0_UnderlyingFundId", "1");
			formCollection.Add("0_Amount", "1");
			formCollection.Add("0_CapitalCallDate", DateTime.MaxValue.ToString());
			formCollection.Add("TotalRows","1");
			return formCollection;
		}
	}
}
