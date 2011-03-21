using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IManagementFeeRateScheduleService {
		void SaveManagementFeeRateSchedule(ManagementFeeRateSchedule managementFeeRateSchedule);
	}
	public class ManagementFeeRateScheduleService : IManagementFeeRateScheduleService {

		#region IManagementFeeRateScheduleService Members

		public void SaveManagementFeeRateSchedule(ManagementFeeRateSchedule managementFeeRateSchedule) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (managementFeeRateSchedule.ManagementFeeRateScheduleID == 0) {
					context.ManagementFeeRateSchedules.AddObject(managementFeeRateSchedule);
				}
				/*else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("ManagementFeeRateSchedules", managementFeeRateSchedule);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, managementFeeRateSchedule);
					}
				}*/
				context.SaveChanges();
			}
		}

		#endregion
	}
}