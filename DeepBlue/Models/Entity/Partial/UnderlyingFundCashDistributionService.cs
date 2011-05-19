using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IUnderlyingFundCashDistributionService {
		void SaveUnderlyingFundCashDistribution(UnderlyingFundCashDistribution underlyingFundCashDistribution);
	}
	public class UnderlyingFundCashDistributionService : IUnderlyingFundCashDistributionService {

		#region IUnderlyingFundCashDistributionService Members

		public void SaveUnderlyingFundCashDistribution(UnderlyingFundCashDistribution underlyingFundCashDistribution) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (underlyingFundCashDistribution.UnderlyingFundCashDistributionID == 0) {
					context.UnderlyingFundCashDistributions.AddObject(underlyingFundCashDistribution);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("UnderlyingFundCashDistributions", underlyingFundCashDistribution);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, underlyingFundCashDistribution);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}