using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IDealUnderlyingFundService {
		void SaveDealUnderlyingFund(DealUnderlyingFund dealUnderlyingFund);
	}
	public class DealUnderlyingFundService : IDealUnderlyingFundService {

		#region IDealUnderlyingFundService Members

		public void SaveDealUnderlyingFund(DealUnderlyingFund dealUnderlyingFund) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (dealUnderlyingFund.DealUnderlyingtFundID == 0) {
					context.DealUnderlyingFunds.AddObject(dealUnderlyingFund);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("DealUnderlyingFunds", dealUnderlyingFund);
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