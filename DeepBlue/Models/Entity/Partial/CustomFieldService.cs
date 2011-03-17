using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

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
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("CustomFields", customField);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, customField);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}