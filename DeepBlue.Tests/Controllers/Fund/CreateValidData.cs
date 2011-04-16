using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Fund;

namespace DeepBlue.Tests.Controllers.Fund {
    public class CreateValidData : Create {

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
            MockFundRepository.Setup(x => x.SaveFund(It.IsAny<DeepBlue.Models.Entity.Fund>())).Returns(new List<Helpers.ErrorInfo>());
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
		public void valid_Fund_name_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("FundName"));
		}

		[Test]
		public void valid_Fund_name_sets_1_error() {
			Assert.IsTrue(test_error_count("FundName", 0));
		}

		[Test]
		public void valid_Fund_taxid_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("TaxId"));
		}

		[Test]
		public void valid_Fund_taxid_sets_1_error() {
			Assert.IsTrue(test_error_count("TaxId", 0));
		}

		[Test]
		public void valid_Fund_inceptiondate_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("InceptionDate"));
		}

		[Test]
		public void valid_Fund_inceptiondate_sets_1_error() {
			Assert.IsTrue(test_error_count("InceptionDate", 0));
		}

		[Test]
		public void valid_Fund_bankname_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("BankName"));
		}

		[Test]
		public void valid_Fund_bankname_sets_1_error() {
			Assert.IsTrue(test_error_count("BankName", 0));
		}


		[Test]
		public void valid_Fund_account_sets_model_error_on_model_state() {
			Assert.IsTrue(test_posted_value("Account"));
		}

		[Test]
		public void valid_Fund_account_sets_1_error() {
			Assert.IsTrue(test_error_count("Account", 0));
		}



		[Test]
		public void returns_back_to_new_view_if_saving_fund_failed() {
			SetFormCollection();
			Assert.IsNotNull(ResultModel);
		}

        #endregion
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
            formCollection.Add("FundName", "Test");
            formCollection.Add("TaxId", "1");
			formCollection.Add("FundStartDate","1/1/1999");
            formCollection.Add("InceptionDate", "1/1/1999");
			formCollection.Add("BankName","Test");
			formCollection.Add("Account","Test"); 
            return formCollection;
        }
    }
}
