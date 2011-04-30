using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Document;
using System.Web;

namespace DeepBlue.Tests.Controllers.Document {
    public class CreateDocumentSearchValidData : CreateDocumentSearch {
        private ModelStateDictionary ModelState {
            get {
                return base.ViewResult.ViewData.ModelState;
            }
        }

        #region Tests for List
        private void SetFormCollection() {
            base.DefaultController.ValueProvider = SetupValueProvider(GetValidformCollection());
			base.ActionResult = base.DefaultController.List(1, 15, "DocumentDate", "desc", "", "", 0, 0, 0, 1);
        }
		#endregion
        #region Tests after model state is invalid
        [Test]
        public void returns_back_to_new_view_if_Search_document_failed() {
            SetFormCollection();
            Assert.IsNotNull(Model);
        }

        #endregion


		private FormCollection GetValidformCollection() {
			FormCollection formCollection = new FormCollection();
			formCollection.Add("pageIndex", "1");
			formCollection.Add("pageSize", "15");
			formCollection.Add("sortName", "DocumentDate");
			formCollection.Add("sortOrder", "desc");
			formCollection.Add("fromDate", "");
			formCollection.Add("toDate", "");
			formCollection.Add("investorId", "0");
			formCollection.Add("fundId", "0");
			formCollection.Add("documentTypeId", "0");
			formCollection.Add("documentStatusId", "1");
			return formCollection;
		}
    }
}
