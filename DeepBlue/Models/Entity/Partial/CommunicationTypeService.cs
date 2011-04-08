using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface ICommunicationTypeService {
		void SaveCommunicationType(CommunicationType communicationType);
	}
	public class CommunicationTypeService : ICommunicationTypeService {

		#region ICommunicationTypeService Members

		public void SaveCommunicationType(CommunicationType communicationType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (communicationType.CommunicationTypeID == 0) {
					context.CommunicationTypes.AddObject(communicationType);
				} else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("CommunicationTypes", communicationType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, communicationType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}