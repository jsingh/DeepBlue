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
	public class CreateUnderlyingFundCapitalCallValidData : CreateUnderlyingFundCapitalCall {
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
			base.ActionResult = base.DefaultController.CreateUnderlyingFundCapitalCall(validFormCollection);
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
		public void valid_underlyingfundcapitalcall_amount_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("Amount"));
		}

		[Test]
		public void valid_underlyingfundcapitalcall_amount_sets_0_error() {
			Assert.IsTrue(test_error_count("Amount", 0));
		}
		
		[Test]
		public void valid_underlyingfundcapitalcall_fundid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("FundId"));
		}

		[Test]
		public void valid_underlyingfundcapitalcall_fundid_sets_0_error() {
			Assert.IsTrue(test_error_count("FundId", 0));
		}
		
		[Test]
		public void valid_underlyingfundcapitalcall_underlyingfundid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("UnderlyingFundId"));
		}

		[Test]
		public void valid_underlyingfundcapitalcall_underlyingfundid_sets_0_error() {
			Assert.IsTrue(test_error_count("UnderlyingFundId", 0));
		}

		[Test]
		public void valid_underlyingfundcapitalcall_noticedate_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("NoticeDate"));
		}

		[Test]
		public void valid_underlyingfundcapitalcall_noticedate_sets_0_error() {
			Assert.IsTrue(test_error_count("NoticeDate", 0));
		}

		[Test]
		public void valid_underlyingfundcapitalcall_receiveddate_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("ReceivedDate"));
		}

		[Test]
		public void valid_underlyingfundcapitalcall_receiveddate_sets_0_error() {
			Assert.IsTrue(test_error_count("ReceivedDate", 0));
		}
 
		
		//[Test]
		//public void valid_fund_results_in_valid_modelstate() {
		//    SetFormCollection();
		//    Assert.IsTrue(base.DefaultController.ModelState.IsValid);
		//}

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
			formCollection.Add("0_UnderlyingFundCapitalCallId", "0");
			formCollection.Add("0_Amount", "1");
			formCollection.Add("0_FundId", "1");
			formCollection.Add("0_UnderlyingFundId", "1");
			formCollection.Add("0_NoticeDate", DateTime.MaxValue.ToString());
			formCollection.Add("0_ReceivedDate", DateTime.MaxValue.ToString());
			formCollection.Add("0_CashDistributionTypeId", "1");
			formCollection.Add("0_PaidDate", DateTime.MaxValue.ToString());
			formCollection.Add("TotalRows","1");
			return formCollection;
		}
	}
}
