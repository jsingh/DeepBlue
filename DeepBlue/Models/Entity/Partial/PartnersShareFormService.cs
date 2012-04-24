using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace DeepBlue.Models.Entity {

	public interface IPartnersShareFormService {
		void SavePartnersShareForm(PartnersShareForm partnersShareForm);
	}

	public class PartnersShareFormService : IPartnersShareFormService {

		#region IPartnersShareFormService Members

		public void SavePartnersShareForm(PartnersShareForm partnersShareForm) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (partnersShareForm.PartnersShareFormID == 0) {
					context.PartnersShareForms.AddObject(partnersShareForm);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("PartnersShareForms", partnersShareForm);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, partnersShareForm);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}