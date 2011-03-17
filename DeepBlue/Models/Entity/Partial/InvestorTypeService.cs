using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IInvestorTypeService {
		void SaveInvestorType(InvestorType investorType);
	}
	public class InvestorTypeService : IInvestorTypeService {

		#region IInvestorTypeService Members

		public void SaveInvestorType(InvestorType investorType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (investorType.InvestorTypeID == 0) {
					context.InvestorTypes.AddObject(investorType);
				} else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("InvestorTypes", investorType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, investorType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}