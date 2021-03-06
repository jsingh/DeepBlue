﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;
using DeepBlue.Models.Document;
using DeepBlue.Helpers;
using DeepBlue.Controllers.Document;

namespace DeepBlue.Tests.Controllers.Document {
	public class NewDocumentSearch :DocumentSearchBase {

        private  CreateModel   Model {
            get {
				return base.ViewResult.ViewData.Model as CreateModel;
            }
        }
        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();
			base.ActionResult = base.DefaultController.New();
        }
 
		[Test]
		public void create_a_new_document() {
			Assert.IsInstanceOfType<ActionResult>(base.ActionResult);
		}
    }
}
