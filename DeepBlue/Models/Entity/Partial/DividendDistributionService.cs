using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {

	public interface IDividendDistributionService {
		void SaveDividendDistribution(DividendDistribution dividendDistribution);
	}

	public class DividendDistributionService : IDividendDistributionService {

		#region IDividendDistributionService Members

		public void SaveDividendDistribution(DividendDistribution dividendDistribution) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (dividendDistribution.DividendDistributionID == 0) {
					context.DividendDistributions.AddObject(dividendDistribution);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("DividendDistributions", dividendDistribution);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, dividendDistribution);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}