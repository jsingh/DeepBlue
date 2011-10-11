using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {

	public interface IDocumentTypeService {
		void SaveDocumentType(DocumentType documentType);
	}

	public class DocumentTypeService : IDocumentTypeService {

		#region IDocumentTypeService Members

		public void SaveDocumentType(DocumentType documentType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (documentType.DocumentTypeID == 0) {
					context.DocumentTypes.AddObject(documentType);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("DocumentTypes", documentType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, documentType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}