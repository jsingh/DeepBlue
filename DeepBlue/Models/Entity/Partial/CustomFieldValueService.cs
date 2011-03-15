using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Models.Entity {
	public interface ICustomFieldValueService {
		void SaveCustomFieldValue(CustomFieldValue customFieldValue);
	}
	public class CustomFieldValueService : ICustomFieldValueService {

		#region ICustomFieldValueService Members

		public void SaveCustomFieldValue(CustomFieldValue customFieldValue) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (customFieldValue.CustomFieldValueID == 0) {
					context.CustomFieldValues.AddObject(customFieldValue);
				} else {
					context.CustomFieldValues.SingleOrDefault(entityType => entityType.CustomFieldValueID == customFieldValue.CustomFieldValueID);
					context.CustomFieldValues.ApplyCurrentValues(customFieldValue);
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}