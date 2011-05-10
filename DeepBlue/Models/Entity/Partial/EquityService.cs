using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IEquityService {
		void SaveEquity(Equity equity);
	}
	public class EquityService : IEquityService {

		#region IEquityService Members

		public void SaveEquity(Equity equity) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (equity.EquityID == 0) {
					context.Equities.AddObject(equity);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("Equities", equity);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, equity);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}