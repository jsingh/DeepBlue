using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IActivityTypeService {
		void SaveActivityType(ActivityType activityType);
	}
	public class ActivityTypeService : IActivityTypeService {

		#region IActivityTypeService Members

		public void SaveActivityType(ActivityType activityType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (activityType.ActivityTypeID == 0) {
					context.ActivityTypes.AddObject(activityType);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("ActivityTypes", activityType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, activityType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}