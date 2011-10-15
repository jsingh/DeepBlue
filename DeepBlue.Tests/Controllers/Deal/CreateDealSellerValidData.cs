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
    public class CreateDealSellerValidData : CreateDealSeller  {

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
			base.ActionResult = base.DefaultController.CreateSellerInfo(GetValidformCollection());
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
		public void valid_dealseller_contactname_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("ContactName"));
		}

		[Test]
		public void valid_dealseller_contactname_sets_0_error() {
			Assert.IsTrue(test_error_count("ContactName", 0));
		}


		[Test]
		public void valid_dealseller_phone_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("Phone"));
		}

		[Test]
		public void valid_dealseller_phone_sets_0_error() {
			Assert.IsTrue(test_error_count("Phone", 0));
		}

		[Test]
		public void valid_deal_fax_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("Fax"));
		}

		[Test]
		public void valid_deal_fax_sets_0_error() {
			Assert.IsTrue(test_error_count("Fax", 0));
		}

		[Test]
		public void valid_dealseller_name_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("SellerName"));
		}

		[Test]
		public void valid_dealseller_name_sets_0_error() {
			Assert.IsTrue(test_error_count("SellerName", 0));
		}

		[Test]
		public void valid_dealseller_companyname_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("CompanyName"));
		}

		[Test]
		public void valid_dealseller_companyname_sets_0_error() {
			Assert.IsTrue(test_error_count("CompanyName", 0));
		}

		[Test]
		public void valid_dealseller_email_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("Email"));
		}

		[Test]
		public void valid_dealseller_email_sets_0_error() {
			Assert.IsTrue(test_error_count("Email", 0));
		}

		[Test]
		public void valid_dealseller_deal_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("DealId"));
		}

		[Test]
		public void valid_dealseller_deal_sets_0_error() {
			Assert.IsTrue(test_error_count("DealId", 0));
		}

		[Test]
		public void returns_back_to_new_view_if_saving_fund_failed() {
			SetFormCollection();
			Assert.IsNotNull(ResultModel);
		}

        #endregion
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
			formCollection.Add("ContactName", "Test");
			formCollection.Add("Phone", "1111111");
			formCollection.Add("Fax", "1111111");
			formCollection.Add("SellerName", "Test");
			formCollection.Add("CompanyName", "Test");
			formCollection.Add("Email", "test@test.com");
			formCollection.Add("DealId", "1");
            return formCollection;
        }
    }
}
