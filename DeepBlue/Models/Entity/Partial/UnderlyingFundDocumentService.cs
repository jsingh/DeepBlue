using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IUnderlyingFundDocumentService {
		void SaveUnderlyingFundDocument(UnderlyingFundDocument underlyingFundDocument);
	}
	public class UnderlyingFundDocumentService : IUnderlyingFundDocumentService {

		#region IUnderlyingFundDocumentService Members

		public void SaveUnderlyingFundDocument(UnderlyingFundDocument underlyingFundDocument) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (underlyingFundDocument.UnderlyingFundDocumentID == 0) {
					context.UnderlyingFundDocuments.AddObject(underlyingFundDocument);
				}
				else {
					EntityKey key;
					object originalItem;
					UnderlyingFundDocument updateUnderlyingFundDocument = context.UnderlyingFundDocuments.SingleOrDefault(deepblueUnderlyingFundDocument => deepblueUnderlyingFundDocument.UnderlyingFundDocumentID == underlyingFundDocument.UnderlyingFundDocumentID);
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("Files", underlyingFundDocument.File);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, underlyingFundDocument.File);
					}
					else {
						updateUnderlyingFundDocument.File = new Models.Entity.File {
							CreatedBy = underlyingFundDocument.File.CreatedBy,
							CreatedDate = underlyingFundDocument.File.CreatedDate,
							EntityID = underlyingFundDocument.File.EntityID,
							FileID = underlyingFundDocument.File.FileID,
							FileName = underlyingFundDocument.File.FileName,
							FilePath = underlyingFundDocument.File.FilePath,
							FileTypeID = underlyingFundDocument.File.FileTypeID,
							LastUpdatedBy = underlyingFundDocument.File.LastUpdatedBy,
							LastUpdatedDate = underlyingFundDocument.File.LastUpdatedDate,
							Size = underlyingFundDocument.File.Size
						};
					}
					// Define an ObjectStateEntry and EntityKey for the current object. 
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("UnderlyingFundDocuments", underlyingFundDocument);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, underlyingFundDocument);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}