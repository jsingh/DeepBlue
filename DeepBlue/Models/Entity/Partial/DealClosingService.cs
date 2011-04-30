using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IDealClosingService {
		void SaveDealClosing(DealClosing dealClosing);
	}
	public class DealClosingService : IDealClosingService {

		#region IDealClosingService Members

		public void SaveDealClosing(DealClosing dealClosing) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (dealClosing.DealClosingID == 0) {
					context.DealClosings.AddObject(dealClosing);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("DealClosings", dealClosing);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, dealClosing);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}