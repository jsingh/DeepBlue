using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IUnderlyingFundStockDistributionService {
		void SaveUnderlyingFundStockDistribution(UnderlyingFundStockDistribution underlyingFundStockDistribution);
	}
	public class UnderlyingFundStockDistributionService : IUnderlyingFundStockDistributionService {

		#region IUnderlyingFundStockDistributionService Members

		public void SaveUnderlyingFundStockDistribution(UnderlyingFundStockDistribution underlyingFundStockDistribution) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (underlyingFundStockDistribution.UnderlyingFundStockDistributionID == 0) {
					context.UnderlyingFundStockDistributions.AddObject(underlyingFundStockDistribution);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("UnderlyingFundStockDistributions", underlyingFundStockDistribution);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, underlyingFundStockDistribution);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}