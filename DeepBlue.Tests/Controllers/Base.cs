using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections;
using System.Security.Principal;
using MbUnit.Framework;
using System.Configuration;
using DeepBlue.Tests;

namespace DeepBlue.Tests.Controllers {

    public class Base {
        private MockSessionState _mockSession = new MockSessionState(new System.Web.SessionState.SessionStateItemCollection());
        public int entityID = 1;
        protected ActionResult ActionResult;
        protected ViewResultBase ViewResult {
            get { return this.ActionResult as ViewResultBase; }
        }

        public void InitializeHTTPContext() {
            #region create our mocks
            var mockHttpContext = new Mock<HttpContextBase>(MockBehavior.Strict);
            var mockHttpResponse = new Mock<HttpResponseBase>();
            var mockHttpRequest = new Mock<HttpRequestBase>();
            // Mock the Session
            var mockSessionStore = new Mock<HttpSessionStateBase>();
            mockSessionStore.SetupAllProperties();
            // karan: 02/15/2011
            // TODO: (A) When we put in authentication based on the user login, we need to mock the principal here
            // Example Code
            // var mockDeepBluePrincipal = new DeepBlue.Model.UserPrincipal(new GenericIdentity("Generic"), new DeepBlue.Model.UserLogin { UserID = 123456, EntityID = entityID });
            // mockDeepBluePrincipal.CurrentUser.EntityID = entityID;
            // mockDeepBluePrincipal.CurrentUser.Name = "karan@researchsqr.com";
            #endregion

            #region set our expectations
            string output = "";
            // Return Mocked Response object from HttpContext
            mockHttpContext.Setup(mhc => mhc.Response).Returns(mockHttpResponse.Object);
            // Return an empty dictionary when Items property is accessed from the HttpContext
            mockHttpContext.Setup(p => p.Items).Returns(new Dictionary<object, object>());
            // Return Mocked Session object from HttpContext
            mockHttpContext.Setup(p => p.Session).Returns(_mockSession);
            // TODO: When (A) is in place, we need to setup the expectation to return the mocked principal from the context
            // mockHttpContext.Setup(p => p.User).Returns(mockDeepBluePrincipal);
            // mockHttpContext.SetupSet(p => p.User);
            // Mock Response.Write
            mockHttpResponse.Setup(mhr => mhr.Write(It.IsAny<string>())).Callback<string>(s => output += s);
            // Mock Request.Form
            mockHttpRequest.Setup(p => p.Form).Returns(new System.Collections.Specialized.NameValueCollection());
            // Mock Request.Params
            mockHttpRequest.Setup(p => p.Params).Returns(new System.Collections.Specialized.NameValueCollection());
            // Set up the mock Request
            mockHttpContext.Setup(p => p.Request).Returns(mockHttpRequest.Object);
            #endregion

            // TODO: If we add the Current Entity to the Context, then we may need to muck these too.
            //Mock Current Entity
            //mockHttpContext.Object.Items.Add("CurrentEntity",
            //    DeepBlue.Fixture.Entity.EntityBuilder.Entity().Build())
            //    .Build());

            //Mock CurrentUser
            //mockHttpContext.Object.Session["CurrentUser"] = mockDeepBluePrincipal;

            //set the mock context
            DeepBlue.Helpers.HttpContextFactory.SetHttpContext(mockHttpContext.Object);
        }

        [SetUp]
        public virtual void Setup() {
            this.InitializeHTTPContext();
        }

        [TearDown]
        public virtual void TearDown() {

        }

        protected T CastModel<T>() {
            return (T)this.ViewResult.ViewData.Model;
        }
    }
}
