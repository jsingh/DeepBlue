using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IDealUnderlyingFundAdjustmentService {
		void SaveDealUnderlyingFundAdjustment(DealUnderlyingFundAdjustment dealUnderlyingFund);
	}
	public class DealUnderlyingFundAdjustmentService : IDealUnderlyingFundAdjustmentService {

		#region IDealUnderlyingFundAdjustmentService Members

		public void SaveDealUnderlyingFundAdjustment(DealUnderlyingFundAdjustment dealUnderlyingFund) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (dealUnderlyingFund.DealUnderlyingFundAdjustmentID == 0) {
					context.DealUnderlyingFundAdjustments.AddObject(dealUnderlyingFund);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("DealUnderlyingFundAdjustments", dealUnderlyingFund);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, dealUnderlyingFund);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}