using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Deal ;

namespace DeepBlue.Tests.Controllers.Deal {
    public class CreateDealSellerInvalidData : CreateDealSeller {
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
			base.DefaultController.ValueProvider = SetupValueProvider(GetInvalidformCollection());
			base.ActionResult = base.DefaultController.CreateSellerInfo(GetInvalidformCollection());
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
		public void invalid_Dealseller_contactname_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("ContactName"));
		}

		[Test]
		public void invalid_Dealseller_contactname_sets_1_error() {
			Assert.IsTrue(test_error_count("ContactName", 1));
		}

		[Test]
		public void invalid_Dealseller_phone_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("Phone"));
		}

		[Test]
		public void invalid_Dealseller_phone_sets_1_error() {
			Assert.IsTrue(test_error_count("Phone", 1));
		}

		[Test]
		public void invalid_Dealseller_fax_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("Fax"));
		}

		[Test]
		public void invalid_Dealseller_fax_sets_1_error() {
			Assert.IsTrue(test_error_count("Fax", 1));
		}
		
		[Test]
		public void invalid_Dealseller_name_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("SellerName"));
		}

		[Test]
		public void invalid_Dealseller_name_sets_1_error() {
			Assert.IsTrue(test_error_count("SellerName", 1));
		}

		[Test]
		public void invalid_Dealseller_companyname_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("CompanyName"));
		}

		[Test]
		public void invalid_Dealseller_companyname_sets_1_error() {
			Assert.IsTrue(test_error_count("CompanyName", 1));
		}

		[Test]
		public void invalid_Dealseller_email_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("Email"));
		}

		[Test]
		public void invalid_Dealseller_email_sets_1_error() {
			Assert.IsTrue(test_error_count("Email", 1));
		}

		[Test]
		public void invalid_Dealseller_Deal_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("DealId"));
		}

		[Test]
		public void invalid_Dealseller_Deal_sets_1_error() {
			Assert.IsTrue(test_error_count("DealId", 1));
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
			formCollection.Add("ContactName", GetString(101));
			formCollection.Add("Phone", GetString(201));
			formCollection.Add("Fax", GetString(201));
			formCollection.Add("SellerName",  GetString(31));
			formCollection.Add("CompanyName", GetString(201));
			formCollection.Add("Email", GetString(201));
			formCollection.Add("DealId", string.Empty);
            return formCollection;
        }
		 
    }
}
