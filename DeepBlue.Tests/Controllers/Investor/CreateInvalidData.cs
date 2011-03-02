using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Investor;

namespace DeepBlue.Tests.Controllers.Investor {
    public class CreateInvalidData : Create {
        private CreateModel Model {
            get {
                return base.ViewResult.ViewData.Model as CreateModel;
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
            base.DefaultController.ValueProvider = SetupValueProvider(null);
            base.ActionResult = base.DefaultController.Create(new FormCollection());
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
        public void invalid_Investor_name_sets_model_error_on_model_state() {
            Assert.IsFalse(test_posted_value("InvestorName"));
        }

        [Test]
        public void invalid_Investor_name_sets_1_error() {
            Assert.IsTrue(test_error_count("InvestorName", 1));
        }

        [Test]
        public void invalid_Investor_name_results_in_invalid_modelstate() {
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
        public void model_state_invalid_redirects_to_new_view() {
            SetModelInvalid();
            Assert.IsNotNull(Model);
        }

        [Test]
        public void model_state_invalid_redirects_new_view_countries_populated() {
            SetModelInvalid();
            // Make sure it is a redirect
            Assert.IsTrue(Model.SelectList.Countries.Count > 0);
        }

        [Test]
        public void model_state_invalid_redirects_new_view_states_populated() {
            SetModelInvalid();
            // Make sure it is a redirect
            Assert.IsTrue(Model.SelectList.States.Count > 0);
        }

        [Test]
        public void model_state_invalid_redirects_new_view_addresstypes_populated() {
            SetModelInvalid();
            // Make sure it is a redirect
            Assert.IsTrue(Model.SelectList.AddressTypes.Count > 0);
        }

        [Test]
        public void model_state_invalid_redirects_new_view_domesticforeigns_populated() {
            SetModelInvalid();
            // Make sure it is a redirect
            Assert.IsTrue(Model.SelectList.DomesticForeigns.Count > 0);
        }

        #endregion

        //private void RequiredFieldDataMissing(CreateModel model) {
        //    model.MemberName = model.Alias = model.Phone = model.Email = model.Address1 = model.City = model.Zip = string.Empty;
        //}

        //private void RequiredFieldDataMissingFromForm(CreateModel model) {
        //    model.MemberName = model.Alias = model.Phone = model.Email = model.Address1 = model.City = model.Zip = string.Empty;
        //}

        //private void RequiredFieldDataMissingFromForm() {
        //    base.DefaultController.ControllerContext.HttpContext.Request.Form.Add("MemberName", string.Empty);
        //    base.DefaultController.ControllerContext.HttpContext.Request.Form.Add("Alias", string.Empty);
        //    base.DefaultController.ControllerContext.HttpContext.Request.Form.Add("Phone", string.Empty);
        //    base.DefaultController.ControllerContext.HttpContext.Request.Form.Add("Email", string.Empty);
        //    base.DefaultController.ControllerContext.HttpContext.Request.Form.Add("Address1", string.Empty);
        //    base.DefaultController.ControllerContext.HttpContext.Request.Form.Add("City", string.Empty);
        //    base.DefaultController.ControllerContext.HttpContext.Request.Form.Add("Zip", string.Empty);
        //}

        private FormCollection GetInvalidformCollection() {
            FormCollection formCollection = new FormCollection();
            formCollection.Add("InvestorName", string.Empty);
            formCollection.Add("Alias", string.Empty);
            formCollection.Add("Phone", string.Empty);
            formCollection.Add("Email", string.Empty);
            formCollection.Add("Address1", string.Empty);
            formCollection.Add("City", string.Empty);
            formCollection.Add("Zip", string.Empty);
            return formCollection;
        }
    }
}
