using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DeepBlue.Helpers {
	public class RemoteAttributeAdapter : DataAnnotationsModelValidator<RemoteUID_Attribute> {

		public RemoteAttributeAdapter(ModelMetadata metadata, ControllerContext context,
			RemoteUID_Attribute attribute) :
			base(metadata, context, attribute) {
		}

		public override IEnumerable<ModelClientValidationRule> GetClientValidationRules() {
			ModelClientValidationRule rule = new ModelClientValidationRule() {
				// Use the default DataAnnotationsModelValidator error message.
				// This error message will be overridden by the string returned by
				// IsUID_Available unless "FAIL"  or "OK" is returned in 
				// the Validation Controller.
				ErrorMessage = ErrorMessage,
				ValidationType = "remoteVal"
			};

			rule.ValidationParameters["url"] = GetUrl();
			rule.ValidationParameters["validateParameterName"] = Attribute.ValidateParameterName;
			string parameters = string.Empty;
			foreach (var para in Attribute.Params) {
				parameters += para + ",";
			}
			if (string.IsNullOrEmpty(parameters) == false) {
				parameters = parameters.Substring(0, parameters.Length - 1);
			}
			rule.ValidationParameters["parameterNames"] = parameters;
			return new ModelClientValidationRule[] { rule };
		}

		private string GetUrl() {
			RouteValueDictionary rvd = new RouteValueDictionary() {
				{ "controller", Attribute.Controller },
				{ "action", Attribute.Action }
			};

			var virtualPath = RouteTable.Routes.GetVirtualPath(ControllerContext.RequestContext,
				Attribute.RouteName, rvd);
			if (virtualPath == null) {
				throw new InvalidOperationException("No route matched!");
			}

			return virtualPath.VirtualPath;
		}
	}


}