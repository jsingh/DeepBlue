using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IFileTypeService {
		void SaveFileType(FileType fileType);
	}
	public class FileTypeService : IFileTypeService {

		#region IFileTypeService Members

		public void SaveFileType(FileType fileType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (fileType.FileTypeID == 0) {
					context.FileTypes.AddObject(fileType);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("FileTypes", fileType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, fileType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}