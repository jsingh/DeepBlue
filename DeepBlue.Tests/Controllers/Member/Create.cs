using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MbUnit.Framework;
using Moq;

namespace DeepBlue.Tests.Controllers.Member {
    public class Create: MemberBase {
        [SetUp]
        public override void Setup() {
            // Arrange
            base.Setup();

            //base.MockRepository.Setup(x => x.GetSomethingFromDatabase(It.IsAny<int>())).Returns(5);
            
            //base.DefaultController.HttpContext.Request.Form.Add("firstname", "first");
            //base.DefaultController.HttpContext.Request.Form.Add("lastname", "last");
            //base.DefaultController.HttpContext.Request.Form.Add("emailaddress", "email@email.com");
            //base.DefaultController.HttpContext.Request.Form.Add("country_code", "US");
            //base.DefaultController.HttpContext.Request.Form.Add("street1", "123 Main St");
            //base.DefaultController.HttpContext.Request.Form.Add("street2", string.Empty);
            //base.DefaultController.HttpContext.Request.Form.Add("city", "city");
            //base.DefaultController.HttpContext.Request.Form.Add("state", "ST");
            //base.ActionResult = base.DefaultController.Create();
        }

        [Test]
        public void create_a_new_member() {
            Assert.IsInstanceOfType<RedirectToRouteResult>(base.ActionResult);
        }
    }
}
