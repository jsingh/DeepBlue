using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Transaction;

namespace DeepBlue.Tests.Controllers.Transaction {
    public class CreateInvestorFundInvalidData : CreateInvestorFund {
        
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
            base.DefaultController.ValueProvider = SetupValueProvider(new FormCollection());
			base.ActionResult = base.DefaultController.CreateInvestorFund(GetInvalidformCollection());
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
        public void invalid_fund_id_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("FundId"));
        }

        [Test]
		public void invalid_fund_id_sets_1_error() {
			Assert.IsTrue(test_error_count("FundId", 1));
        }

        [Test]
		public void invalid_fund_id_results_in_invalid_modelstate() {
            SetFormCollection();
            Assert.IsFalse(base.DefaultController.ModelState.IsValid);
        }

		[Test]
		public void invalid_fund_closing_id_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("FundClosingId"));
		}

		[Test]
		public void invalid_fund_closing_id_sets_1_error() {
			Assert.IsTrue(test_error_count("FundClosingId", 1));
		}

		[Test]
		public void invalid_fund_closing_id_results_in_invalid_modelstate() {
			SetFormCollection();
			Assert.IsFalse(base.DefaultController.ModelState.IsValid);
		}

		[Test]
		public void invalid_totalcommitment_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("FundClosingId"));
		}

		[Test]
		public void invalid_totalcommitment_sets_1_error() {
			Assert.IsTrue(test_error_count("FundClosingId", 1));
		}

		[Test]
		public void invalid_totalcommitment_results_in_invalid_modelstate() {
			SetFormCollection();
			Assert.IsFalse(base.DefaultController.ModelState.IsValid);
		}
				
		[Test]
		public void invalid_fund_committeddate_sets_model_error_on_model_state() {
			Assert.IsFalse(test_posted_value("CommittedDate"));
		}

		[Test]
		public void invalid_fund_committeddate_sets_1_error() {
			Assert.IsTrue(test_error_count("CommittedDate", 1));
		}

		[Test]
		public void invalid_fund_committeddate_results_in_invalid_modelstate() {
			SetFormCollection();
			Assert.IsFalse(base.DefaultController.ModelState.IsValid);
		}


		private FormCollection GetInvalidformCollection() {
			FormCollection formCollection = new FormCollection();
			formCollection.Add("FundId", string.Empty);
			formCollection.Add("FundClosingId", string.Empty);
			formCollection.Add("CommittedDate", string.Empty);
			return formCollection;
		}
        #endregion

 
        
    }
}
