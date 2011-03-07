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
using DeepBlue.Helpers;

namespace DeepBlue.Tests {


    public class Base {
        private MockSessionState _mockSession = new MockSessionState(new System.Web.SessionState.SessionStateItemCollection());
        public int entityID = 1;
        protected ActionResult ActionResult;
        protected ViewResultBase ViewResult {
            get { return this.ActionResult as ViewResultBase; }
        }

        protected IEnumerable<ErrorInfo> ServiceErrors { get; set; }

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

        /// <summary>
        /// Used for Controller tests. Targets the ModelState to find if there are any errors
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <param name="errorCount"></param>
        /// <param name="controlName"></param>
        /// <returns></returns>
        protected bool IsValid(out string errorMsg, out int errorCount, string controlName) {
            errorMsg = string.Empty;
            errorCount = 0;
            ModelStateDictionary modelState = this.ViewResult.ViewData.ModelState;

            if (modelState != null) {
                errorCount = 0;
                foreach (string key in modelState.Keys) {
                    // If we are not looking for a spacific validation control, or if we are looking for a specific control, and this this that control(key)
                    if (string.IsNullOrEmpty(controlName) || (controlName.Equals(key))) {
                        ModelState ms = modelState[key];
                        if (ms.Errors.Count > 0) {
                            errorCount++;
                            foreach (ModelError error in ms.Errors) {
                                errorMsg += error.ErrorMessage + " ";
                            }
                        }
                    }
                }
            }
            return errorCount == 0;
        }

        protected bool IsValid(string controlName) {
            string errorMsg = string.Empty;
            int errorCount = 0;
            return IsValid(out errorMsg, out errorCount, controlName);
        }

        protected bool IsValid(string controlName, out int errorCount) {
            string errorMsg = string.Empty;
            return IsValid(out errorMsg, out errorCount, controlName);
        }


        /// <summary>
        /// Used for unit testing Models. Targets the DataAnnotations attributes
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <param name="errorCount"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool IsModelValid(out string errorMsg, out int errorCount, string propertyName) {
            errorMsg = string.Empty;
            errorCount = 0;
            if (this.ServiceErrors == null) {
                return true;
            }


            foreach (ErrorInfo error in this.ServiceErrors) {
                // If we are not looking for a spacific validation control, or if we are looking for a specific control, and this this that control(key)
                if (string.IsNullOrEmpty(propertyName) || (propertyName.Equals(error.PropertyName))) {
                    errorCount++;
                    errorMsg += error.ErrorMessage + " ";
                }
            }

            return errorCount == 0;
        }

        protected string GetString(int length) {
            string str = string.Empty;
            for (int i = 0; i < length; i++) {
                str += "z";
            }
            return str;
        }

        /// <summary>
        /// The following exception was being thrown when unit testing code that uses UpdateModel or TryUpdateModel:
        /// Exception: Value cannot be null Parameter name: collection.
        /// The following code was being used
        /// unit test code:
        /// base.ActionResult = base.DefaultController.Create(new FormCollection());
        /// 
        /// Controller code: [MemberController]
        /// public ActionResult Create( FormCollection collection) {
        /// CreateModel model = new CreateModel();
        /// this.TryUpdateModel(model);
        /// 
        /// Solution:
        /// ValueProvider was returning null despite my best efforts to mock the HttpRequest object in the controller’s context (as seen in a bazillion examples online). As it turns out, in MVC2, 
        /// you now have to create a ValueProviderCollection and assign it directly to your controller in order to mock the form post. 
        /// The following code does the trick. Use it in your test like this
        /// controller.ValueProvider = SetupValueProvider(formValues);
        /// </summary>
        /// <param name="formValues"></param>
        /// <returns></returns>
        protected ValueProviderCollection SetupValueProvider(FormCollection form) {
            List<IValueProvider> valueProviders = new List<IValueProvider>();
            valueProviders.Add(form);
            return new ValueProviderCollection(valueProviders);
        }
        //protected ValueProviderCollection SetupValueProvider(Dictionary<string, string> formValues) {
        //    List<IValueProvider> valueProviders = new List<IValueProvider>();

        //    FormCollection form = new FormCollection();
        //    if (formValues != null) {
        //        foreach (string key in formValues.Keys) {
        //            form.Add(key, formValues[key]);
        //        }
        //    }

        //    valueProviders.Add(form);
        //    return new ValueProviderCollection(valueProviders);
        //}
    }
}
