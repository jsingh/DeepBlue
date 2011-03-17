using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

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
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("CustomFieldValues", customFieldValue);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, customFieldValue);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}