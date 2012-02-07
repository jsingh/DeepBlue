using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IDealFundDocumentService {
		void SaveDealFundDocument(DealFundDocument dealFundDocument);
	}
	public class DealFundDocumentService : IDealFundDocumentService {

		#region IDealFundDocumentService Members

		public void SaveDealFundDocument(DealFundDocument dealFundDocument) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (dealFundDocument.DealFundDocumentID == 0) {
					context.DealFundDocuments.AddObject(dealFundDocument);
				}
				else {
					EntityKey key;
					object originalItem;
					DealFundDocument updateDealFundDocument = context.DealFundDocumentsTable.SingleOrDefault(deepblueDealFundDocument => deepblueDealFundDocument.DealFundDocumentID == dealFundDocument.DealFundDocumentID);
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("Files", dealFundDocument.File);
					if (context.TryGetObjectByKey(key, out originalItem)) {
						context.ApplyCurrentValues(key.EntitySetName, dealFundDocument.File);
					}
					else {
						updateDealFundDocument.File = new Models.Entity.File {
							CreatedBy = dealFundDocument.File.CreatedBy,
							CreatedDate = dealFundDocument.File.CreatedDate,
							EntityID = dealFundDocument.File.EntityID,
							FileID = dealFundDocument.File.FileID,
							FileName = dealFundDocument.File.FileName,
							FilePath = dealFundDocument.File.FilePath,
							FileTypeID = dealFundDocument.File.FileTypeID,
							LastUpdatedBy = dealFundDocument.File.LastUpdatedBy,
							LastUpdatedDate = dealFundDocument.File.LastUpdatedDate,
							Size = dealFundDocument.File.Size
						};
					}
					// Define an ObjectStateEntry and EntityKey for the current object. 
					key = default(EntityKey);
					originalItem = null;
					key = context.CreateEntityKey("DealFundDocuments", dealFundDocument);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, dealFundDocument);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}