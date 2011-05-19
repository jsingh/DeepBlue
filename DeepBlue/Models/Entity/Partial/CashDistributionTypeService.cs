using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface ICashDistributionTypeService {
		void SaveCashDistributionType(CashDistributionType cashDistributionType);
	}
	public class CashDistributionTypeService : ICashDistributionTypeService {

		#region ICashDistributionTypeService Members

		public void SaveCashDistributionType(CashDistributionType cashDistributionType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (cashDistributionType.CashDistributionTypeID == 0) {
					context.CashDistributionTypes.AddObject(cashDistributionType);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("CashDistributionTypes", cashDistributionType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, cashDistributionType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}