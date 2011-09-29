using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace DeepBlue.Models.Entity {

	public interface IAnnualMeetingHistoryService {
		void SaveAnnualMeetingHistory(AnnualMeetingHistory annualMeetingHistory);
	}

	public class AnnualMeetingHistoryService : IAnnualMeetingHistoryService {

		#region IAnnualMeetingHistoryService Members

		public void SaveAnnualMeetingHistory(AnnualMeetingHistory annualMeetingHistory) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (annualMeetingHistory.AnnualMeetingHistroyID == 0) {
					context.AnnualMeetingHistories.AddObject(annualMeetingHistory);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("AnnualMeetingHistories", annualMeetingHistory);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, annualMeetingHistory);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}