using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Entity;
using DeepBlue.Helpers;

namespace DeepBlue.Tests.Models.Document {
    public class DocumentValidDataTest : DocumentTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultFundDocument, true);
			this.ServiceErrors = DefaultFundDocument.Save();
        }
        [Test]
        public void create_a_new_document_with_documenttype_id_passes() {
            Assert.IsTrue(IsPropertyValid("DocumentTypeID"));
        }

		[Test]
		public void create_a_new_document_with_document_date_passes() {
			Assert.IsTrue(IsPropertyValid("DocumentDate"));
		}

		[Test]
		public void create_a_new_document_with_valid_investorid_passes() {
			Assert.IsTrue(IsPropertyValid("InvestorId"));
		}
    }
}