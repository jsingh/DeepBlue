using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Reflection;

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
									  select new ErrorInfo(buddyProp.Name, attribute.FormatErrorMessage(attribute.ErrorMessage), instance)).ToList();
			// Add in the class level custom attributes
			IEnumerable<ErrorInfo> classErrors = from attribute in TypeDescriptor.GetAttributes(buddyClassOrModelClass).OfType<ValidationAttribute>()
												 where !attribute.IsValid(instance)
												 select new ErrorInfo("ClassLevelCustom", attribute.FormatErrorMessage(attribute.ErrorMessage), instance);

			errors.AddRange(classErrors);

			string tableName = instance.GetType().Name;
			PropertyInfo property = instance.GetType().GetProperty("EntityID");
			bool isEntityColumnExist = (property != null);
			if (isEntityColumnExist) {
				List<ErrorInfo> entityErrors = new List<ErrorInfo>();
				string expression = string.Empty;
				int entityID = 0;
				object value = property.GetValue(instance, null);
				if (value != null) {
					int.TryParse(value.ToString(), out entityID);
				}
				EntityPermission permission = null;
				Table table;
				Enum.TryParse(tableName, out table);
				if (table != Table.NULL && table != Table.ENTITY) {
					permission = EntityHelper.Permissions.GetEntityPermission(table);
				}
				if (permission != null && entityID > 0) {
					if (permission.IsSystemEntity == false && permission.IsOtherEntity == false) {
						entityErrors.Add(new ErrorInfo("EntityID", "Entity permission disabled"));
					}
					else {
						if (permission.IsSystemEntity == true) {
							property.SetValue(instance, (int)ConfigUtil.SystemEntityID, null);
						}
						else if (entityID != Authentication.CurrentEntity.EntityID) {
							if (!(Authentication.IsSystemEntityUser && table == Table.USER)) {
								entityErrors.Add(new ErrorInfo("EntityID", "Invalid EntityID"));
							}
						}
					}
				}
				if (entityErrors.Count() > 0) {
					errors.AddRange((from err in entityErrors select new ErrorInfo(err.PropertyName, err.ErrorMessage)).ToArray());
				}
			}
			return errors.AsEnumerable();
		}

		public static string GetErrorInfo(IEnumerable<ErrorInfo> errorInfo) {
			StringBuilder errors = new StringBuilder();
			if (errorInfo != null) {
				foreach (var err in errorInfo.ToList()) {
					errors.Append(err.ErrorMessage + "\n");
				}
			}
			return errors.ToString();
		}

	}

	public class ErrorInfo {
		public ErrorInfo(string propertyName, string errorMessage) {
			this.PropertyName = propertyName;
			this.ErrorMessage = errorMessage;
		}
		public ErrorInfo(string propertyName, string errorMessage, object onObject) {
			this.PropertyName = propertyName;
			this.ErrorMessage = errorMessage;
			this.Object = onObject;
		}

		public string ErrorMessage { get; set; }
		public object Object { get; set; }
		public string PropertyName { get; set; }
	}

}