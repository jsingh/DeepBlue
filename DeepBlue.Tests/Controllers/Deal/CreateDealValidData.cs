using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Deal ;
using DeepBlue.Models.Admin;

namespace DeepBlue.Tests.Controllers.Deal {
    public class CreateDealValidData : CreateDeal  {

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
			// Test if the SaveFund call fails
			MockDealRepository.Setup(x => x.SaveDeal(It.IsAny<DeepBlue.Models.Entity.Deal>())).Returns(new List<Helpers.ErrorInfo>());
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
            base.ActionResult = base.DefaultController.Create(GetValidformCollection());
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
		public void valid_deal_fundid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("FundId"));
		}

		[Test]
		public void valid_deal_fundid_sets_1_error() {
			Assert.IsTrue(test_error_count("FundId", 0));
		}


		[Test]
		public void valid_deal_dealnumber_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("DealNumber"));
		}

		[Test]
		public void valid_deal_dealnumber_sets_1_error() {
			Assert.IsTrue(test_error_count("DealNumber", 0));
		}

		[Test]
		public void valid_deal_purchasetypeid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("PurchaseTypeId"));
		}

		[Test]
		public void valid_deal_purchasetypeid_sets_1_error() {
			Assert.IsTrue(test_error_count("PurchaseTypeId", 0));
		}

		[Test]
		public void valid_deal_dealname_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("DealName"));
		}

		[Test]
		public void valid_deal_dealname_sets_1_error() {
			Assert.IsTrue(test_error_count("DealName", 0));
		}


		[Test]
		public void returns_back_to_new_view_if_saving_fund_failed() {
			SetFormCollection();
			Assert.IsNotNull(ResultModel);
		}

        #endregion
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
            formCollection.Add("FundID", "1");
			formCollection.Add("DealNumber", "1");
			formCollection.Add("PurchaseTypeID", "1");
			formCollection.Add("DealName", "Test");
            return formCollection;
        }
    }
}
