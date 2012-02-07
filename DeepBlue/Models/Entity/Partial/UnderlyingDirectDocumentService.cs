using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IUnderlyingDirectDocumentService {
		void SaveUnderlyingDirectDocument(UnderlyingDirectDocument underlyingDirectDocument);
	}
	public class UnderlyingDirectDocumentService : IUnderlyingDirectDocumentService {

		#region IUnderlyingDirectDocumentService Members

		public void SaveUnderlyingDirectDocument(UnderlyingDirectDocument underlyingDirectDocument) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (underlyingDirectDocument.UnderlyingDirectDocumentID == 0) {
					context.UnderlyingDirectDocuments.AddObject(underlyingDirectDocument);
				}
				else {
					EntityKey key;
					object originalItem;
					UnderlyingDirectDocument updateUnderlyingDirectDocument = context.UnderlyingDirectDocumentsTable.SingleOrDefault(deepblueUnderlyingDirectDocument => deepblueUnderlyingDirectDocument.UnderlyingDirectDocumentID == underlyingDirectDocument.UnderlyingDirectDocumentID);
					/* Contact & Communication */
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("Files", underlyingDirectDocument.File);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, underlyingDirectDocument.File);
					}
					else {
						updateUnderlyingDirectDocument.File = new Models.Entity.File {
							CreatedBy = underlyingDirectDocument.File.CreatedBy,
							CreatedDate = underlyingDirectDocument.File.CreatedDate,
							EntityID = underlyingDirectDocument.File.EntityID,
							FileID = underlyingDirectDocument.File.FileID,
							FileName = underlyingDirectDocument.File.FileName,
							FilePath = underlyingDirectDocument.File.FilePath,
							FileTypeID = underlyingDirectDocument.File.FileTypeID,
							LastUpdatedBy = underlyingDirectDocument.File.LastUpdatedBy,
							LastUpdatedDate = underlyingDirectDocument.File.LastUpdatedDate,
							Size = underlyingDirectDocument.File.Size
						};
					}
					// Define an ObjectStateEntry and EntityKey for the current object. 
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("UnderlyingDirectDocuments", underlyingDirectDocument);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, underlyingDirectDocument);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}