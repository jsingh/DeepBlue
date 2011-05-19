using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface ICashDistributionService {
		void SaveCashDistribution(CashDistribution cashDistribution);
	}

	public class CashDistributionService : ICashDistributionService {
		public void SaveCashDistribution(CashDistribution cashDistribution) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (cashDistribution.CashDistributionID == 0) {
					context.CashDistributions.AddObject(cashDistribution);
				}
				else {
					//Update cashDistribution,cashDistribution account values
					//Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key;
					object originalItem;
					key = default(EntityKey);
					key = context.CreateEntityKey("CashDistributions", cashDistribution);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, cashDistribution);
					}
				}
				context.SaveChanges();
			}
		}
	}
}