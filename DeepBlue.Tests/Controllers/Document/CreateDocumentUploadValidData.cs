using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Document;

namespace DeepBlue.Tests.Controllers.Document {
    public class CreateDocumentUploadValidData : CreateDocumentUpload {
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
       
        private FormCollection GetValidformCollection() {
            FormCollection formCollection = new FormCollection();
            formCollection.Add("DocumentTypeID", "1");
			formCollection.Add("DocumentDate", "2010-3-21");
			formCollection.Add("InvestorID", "1");
            return formCollection;
        }
    }
}
