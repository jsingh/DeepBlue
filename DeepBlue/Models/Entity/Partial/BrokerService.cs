using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace DeepBlue.Models.Entity {
	public interface IBrokerService {
		void SaveBroker(Broker broker);
	}

	public class BrokerService : IBrokerService {

		#region IBrokerService Members

		public void SaveBroker(Broker broker) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (broker.BrokerID == 0) {
					context.Brokers.AddObject(broker);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("Brokers", broker);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, broker);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
 
}