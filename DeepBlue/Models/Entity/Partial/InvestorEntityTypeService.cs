using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IInvestorEntityTypeService {
		void SaveInvestorEntityType(InvestorEntityType investorEntityType);
	}
	public class InvestorEntityTypeService : IInvestorEntityTypeService {

		#region IInvestorEntityTypeService Members

		public void SaveInvestorEntityType(InvestorEntityType investorEntityType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (investorEntityType.InvestorEntityTypeID == 0) {
					context.InvestorEntityTypes.AddObject(investorEntityType);
				} else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("InvestorEntityTypes", investorEntityType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, investorEntityType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}