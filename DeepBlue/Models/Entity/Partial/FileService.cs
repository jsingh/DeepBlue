using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IFileService {
		void SaveFile(File file);
	}
	public class FileService : IFileService {

		#region IFileService Members

		public void SaveFile(File file) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (file.FileID == 0) {
					context.Files.AddObject(file);
				}
				else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("Files", file);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, file);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}