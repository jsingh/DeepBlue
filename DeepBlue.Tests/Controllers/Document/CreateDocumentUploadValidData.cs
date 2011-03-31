﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Document;
using System.Web;

namespace DeepBlue.Tests.Controllers.Document {
    public class CreateDocumentUploadValidData : CreateDocumentUpload {
        private ModelStateDictionary ModelState {
            get {
                return base.ViewResult.ViewData.ModelState;
            }
        }

        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
            // Test if the SaveInvestor call fails
            MockRepository.Setup(x => x.SaveDocument(It.IsAny<DeepBlue.Models.Entity.InvestorFundDocument>())).Returns(new List<Helpers.ErrorInfo>());
        }

        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
            base.ActionResult = base.DefaultController.Create(GetValidformCollection());
        }

        #region Tests after model state is invalid
        

        [Test]
        public void returns_back_to_new_view_if_saving_document_failed() {
            SetFormCollection();
            Assert.IsNotNull(Model);
        }

        #endregion

        #region Tests where form collection doesnt have the required values. Tests for DataAnnotations
        private bool test_posted_value(string parameterName)
        {
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
        private bool test_error_count(string parameterName, int errorCount)
        {
            SetFormCollection();
            int errors = 0;
            IsValid(parameterName, out errors);
            return errorCount == errors;
        }

        [Test]
        public void valid_DocumentType_id_sets_model_error_on_model_state()
        {
            Assert.IsTrue(test_posted_value("DocumentTypeId"));
        }

        [Test]
        public void valid_DocumentType_id_sets_1_error()
        {
            Assert.IsTrue(test_error_count("DocumentTypeId", 0));
        }

        [Test]
        public void valid_DocumentDate_sets_model_error_on_model_state()
        {
            Assert.IsTrue(test_posted_value("DocumentDate"));
        }

        [Test]
        public void valid_DocumentDate_sets_1_error()
        {
            Assert.IsTrue(test_error_count("DocumentDate", 0));
        }

        //[Test]
        //public void valid_Document_results_valid_modelstate()
        //{
        //    SetFormCollection();
        //    Assert.IsTrue(base.DefaultController.ModelState.IsValid);
        //}

        #endregion
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
			formCollection.Add("DocumentTypeID", "1");
			formCollection.Add("DocumentDate", "1/1/1900");
			formCollection.Add("InvestorId", "1");
            return formCollection;
        }
    }
}
