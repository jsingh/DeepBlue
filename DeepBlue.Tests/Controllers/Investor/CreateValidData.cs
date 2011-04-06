﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Investor;

namespace DeepBlue.Tests.Controllers.Investor
{
    public class CreateValidData : Create
    {
        private ModelStateDictionary ModelState
        {
            get
            {
                return base.ViewResult.ViewData.ModelState;
            }
        }

        [SetUp]
        public override void Setup()
        {
            // Arrange
            base.Setup();
            // Test if the SaveInvestor call fails
            MockRepository.Setup(x => x.SaveInvestor(It.IsAny<DeepBlue.Models.Entity.Investor>())).Returns(new List<Helpers.ErrorInfo>());
        }

        private void SetFormCollection()
        {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
            base.ActionResult = base.DefaultController.Create(GetValidformCollection());
        }
        #region Tests after model state is invalid

        [Test]
        //public void returns_back_to_new_view_if_saving_investor_failed() {
        //    SetFormCollection();
        //    Assert.IsNotNull(Model);
        //}



        #endregion

        /// <summary>
        /// After the TryUpdateModel tries to update the model, see if the error counts is the same as the DataAnnotations ValidationAttribute count
        /// on the property
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="errorCount"></param>
        /// <returns></returns>
        private bool test_error_count(string parameterName, int errorCount)
        {
            SetFormCollection();
            int errors = 0;
            IsValid(parameterName, out errors);
            return errorCount == errors;
        }

        private bool test_posted_value(string parameterName)
        {
            SetFormCollection();
            return IsValid(parameterName);
        }

        [Test]
        public void valid_Investor_name_sets_model_error_on_model_state()
        {
			Assert.IsTrue(test_posted_value("InvestorName"));
        }

        [Test]
        public void valid_Investor_name_sets_1_error()
        {
             Assert.IsTrue(test_error_count("InvestorName", 0));
        }

        [Test]
        public void valid_Investor_name_results_in_valid_modelstate()
        {
            SetFormCollection();
            Assert.IsTrue(base.DefaultController.ModelState.IsValid);
        }

        #region Tests after model state is invalid
        private void SetModelInvalid()
        {
            base.DefaultController.ModelState.AddModelError(string.Empty, string.Empty);
            SetFormCollection();
        }

        [Test]
        public void model_state_valid_redirects_to_new_view()
        {
            SetModelInvalid();
            Assert.IsNotNull(Model);
        }



        #endregion

        private FormCollection GetValidformCollection()
        {
            FormCollection formCollection = new FormCollection();
            formCollection.Add("InvestorName", "n/a");
            formCollection.Add("Alias", "n/a");
			formCollection.Add("Phone", "2547331111");
            formCollection.Add("Email", "test@email.com");
            formCollection.Add("Address1", "123 Main street");
            formCollection.Add("City", "New York");
            formCollection.Add("StateOfResidency", "1");
            formCollection.Add("EntityType", "1");
            formCollection.Add("SocialSecurityTaxId", "111-11-1111");
            formCollection.Add("Zip", "11005");
            return formCollection;
        }
    }
}
