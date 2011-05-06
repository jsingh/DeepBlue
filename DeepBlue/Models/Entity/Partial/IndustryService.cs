using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IIndustryService {
		void SaveIndustry(Industry industry);
	}
	public class IndustryService : IIndustryService {

		#region IIndustryService Members

		public void SaveIndustry(Industry industry) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (industry.IndustryID == 0) {
					context.Industries.AddObject(industry);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("Industries", industry);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, industry);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}