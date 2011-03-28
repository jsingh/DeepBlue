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
    public class DocumentInvalidDataTest : DocumentTest {

        [SetUp]
        public override void Setup() {
            base.Setup();
			Create_Data(DefaultFundDocument, false);
			this.ServiceErrors = DefaultFundDocument.Save();
        }

        [Test]
        public void create_a_new_document_without_documenttype_id_throws_error() {
			Assert.IsFalse(IsPropertyValid("DocumentTypeID"));
        }

		[Test]
		public void create_a_new_document_without_document_date_throws_error() {
			Assert.IsFalse(IsPropertyValid("DocumentDate"));
		}

		[Test]
		public void create_a_new_document_without_valid_documentinvestorid_throws_error() {
			Assert.IsFalse(IsPropertyValid("InvestorID"));
		}
    }
}