using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface ICommunicationGroupingService {
		void SaveCommunicationGrouping(CommunicationGrouping communicationGrouping);
	}
	public class CommunicationGroupingService : ICommunicationGroupingService {

		#region ICommunicationGroupingService Members

		public void SaveCommunicationGrouping(CommunicationGrouping communicationGrouping) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (communicationGrouping.CommunicationGroupingID == 0) {
					context.CommunicationGroupings.AddObject(communicationGrouping);
				} else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("CommunicationGroupings", communicationGrouping);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, communicationGrouping);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}