using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DeepBlue.Helpers {
    public class ValidationHelper {
        /// <summary>
        /// Get any errors associated with the model also investigating any rules dictated by attached Metadata buddy classes.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static IEnumerable<ErrorInfo> Validate(object instance) {
            var metadataAttrib = instance.GetType().GetCustomAttributes(typeof(MetadataTypeAttribute), true).OfType<MetadataTypeAttribute>().FirstOrDefault();
            var buddyClassOrModelClass = metadataAttrib != null ? metadataAttrib.MetadataClassType : instance.GetType();
            var buddyClassProperties = TypeDescriptor.GetProperties(buddyClassOrModelClass).Cast<PropertyDescriptor>();
            var modelClassProperties = TypeDescriptor.GetProperties(instance.GetType()).Cast<PropertyDescriptor>();

            List<ErrorInfo> errors = (from buddyProp in buddyClassProperties
                                      join modelProp in modelClassProperties on buddyProp.Name equals modelProp.Name
                                      from attribute in buddyProp.Attributes.OfType<ValidationAttribute>()
                                      where !attribute.IsValid(modelProp.GetValue(instance))
                                      select new ErrorInfo(buddyProp.Name, attribute.FormatErrorMessage(string.Empty), instance)).ToList();

            // Add in the class level custom attributes
            IEnumerable<ErrorInfo> classErrors = from attribute in TypeDescriptor.GetAttributes(buddyClassOrModelClass).OfType<ValidationAttribute>()
                                                 where !attribute.IsValid(instance)
                                                 select new ErrorInfo("ClassLevelCustom", attribute.FormatErrorMessage(string.Empty), instance);

            errors.AddRange(classErrors);
            return errors.AsEnumerable();
        }
    }
    public class ErrorInfo {
        public ErrorInfo(string propertyName, string errorMessage) {
            this.PropertyName = propertyName;
            this.ErrorMessage = ErrorMessage;
        }
        public ErrorInfo(string propertyName, string errorMessage, object onObject) {
            this.PropertyName = propertyName;
            this.ErrorMessage = ErrorMessage;
            this.Object = onObject;
        }

        public string ErrorMessage { get; set; }
        public object Object { get; set; }
        public string PropertyName { get; set; }
    }
}