using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Entity {
	public interface ICustomFieldService {
		void SaveCustomField(CustomField customField);
	}
	public class CustomFieldService : ICustomFieldService {

		#region ICustomFieldService Members

		public void SaveCustomField(CustomField customField) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (customField.CustomFieldID == 0) {
					context.CustomFields.AddObject(customField);
				} else {
					context.CustomFields.SingleOrDefault(entityType => entityType.CustomFieldID == customField.CustomFieldID);
					context.CustomFields.ApplyCurrentValues(customField);
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}