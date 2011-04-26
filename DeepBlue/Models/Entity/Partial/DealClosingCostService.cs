using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IDealClosingCostService {
		void SaveDealClosingCost(DealClosingCost dealClosingCost);
	}
	public class DealClosingCostService : IDealClosingCostService {

		#region IDealClosingCostService Members

		public void SaveDealClosingCost(DealClosingCost dealClosingCost) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (dealClosingCost.DealClosingCostID == 0) {
					context.DealClosingCosts.AddObject(dealClosingCost);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("DealClosingCosts", dealClosingCost);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, dealClosingCost);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}