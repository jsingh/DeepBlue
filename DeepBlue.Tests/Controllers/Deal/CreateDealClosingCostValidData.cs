using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Deal ;

namespace DeepBlue.Tests.Controllers.Deal {
    public class CreateDealClosingCostValidData : CreateDealClosingCost  {

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
			MockDealRepository.Setup(x => x.SaveDealClosingCost(It.IsAny<DeepBlue.Models.Entity.DealClosingCost >())).Returns(new List<Helpers.ErrorInfo>());
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
		public void valid_Deal_amount_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("Amount"));
		}

		[Test]
		public void valid_Deal_amount_sets_1_error() {
			Assert.IsTrue(test_error_count("Amount", 0));
		}

		[Test]
		public void valid_Deal_closingcosttypeid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("DealClosingCostTypeID"));
		}

		[Test]
		public void valid_Deal_closingcosttypeid_sets_1_error() {
			Assert.IsTrue(test_error_count("DealClosingCostTypeID", 0));
		}

		[Test]
		public void valid_Deal_id_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("DealID"));
		}

		[Test]
		public void valid_Deal_id_sets_1_error() {
			Assert.IsTrue(test_error_count("DealID", 0));
		}

		[Test]
		public void valid_Deal_date_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("Date"));
		}

		[Test]
		public void valid_Deal_date_sets_1_error() {
			Assert.IsTrue(test_error_count("Date", 0));
		}


		[Test]
		public void returns_back_to_new_view_if_saving_fund_failed() {
			SetFormCollection();
			Assert.IsNotNull(ResultModel);
		}

        #endregion
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
			formCollection.Add("Amount", "10000");
			formCollection.Add("DealClosingCostTypeID", "1");
			formCollection.Add("DealID", "1");
			formCollection.Add("Date", "1/1/1999");
            return formCollection;
        }
    }
}
