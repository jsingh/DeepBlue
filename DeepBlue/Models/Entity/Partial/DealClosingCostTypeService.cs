using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IDealClosingCostTypeService {
		void SaveDealClosingCostType(DealClosingCostType dealClosingCostType);
	}
	public class DealClosingCostTypeService : IDealClosingCostTypeService {

		#region IDealClosingCostTypeService Members

		public void SaveDealClosingCostType(DealClosingCostType dealClosingCostType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (dealClosingCostType.DealClosingCostTypeID == 0) {
					context.DealClosingCostTypes.AddObject(dealClosingCostType);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("DealClosingCostTypes", dealClosingCostType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, dealClosingCostType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}