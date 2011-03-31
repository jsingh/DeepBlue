using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface ICapitalDistributionService {
		void SaveCapitalDistribution(CapitalDistribution capitalCall);
	}

	public class CapitalDistributionService : ICapitalDistributionService {
		public void SaveCapitalDistribution(CapitalDistribution capitalCall) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (capitalCall.CapitalDistributionID == 0) {
					context.CapitalDistributions.AddObject(capitalCall);
				} else {
					//Update capitalCall,capitalCall account values
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key;
					object originalItem;
					foreach (var item in capitalCall.CapitalDistributionLineItems) {
						key = default(EntityKey);
						key = context.CreateEntityKey("CapitalDistributionLineItems", item);
						if (context.TryGetObjectByKey(key, out originalItem)) {
							context.ApplyCurrentValues(key.EntitySetName, item);
						}
					}
					key = default(EntityKey);
					key = context.CreateEntityKey("CapitalDistributions", capitalCall);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, capitalCall);
					}
				}
				context.SaveChanges();
			}
		}
	}
}