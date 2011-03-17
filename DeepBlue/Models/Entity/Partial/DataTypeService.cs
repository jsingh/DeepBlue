using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DeepBlue.Models.Entity {
	public interface IDataTypeService {
		void SaveDataType(DataType dataType);
	}
	public class DataTypeService : IDataTypeService {

		#region IDataTypeService Members

		public void SaveDataType(DataType dataType) {
			using (DeepBlueEntities context = new DeepBlueEntities()) {
				if (dataType.DataTypeID == 0) {
					context.DataTypes.AddObject(dataType);
				} else {
					// Define an ObjectStateEntry and EntityKey for the current object. 
					EntityKey key = default(EntityKey);
					object originalItem = null;
					key = context.CreateEntityKey("DataTypes", dataType);
					// Get the original item based on the entity key from the context 
					// or from the database. 
					if (context.TryGetObjectByKey(key, out originalItem)) {
						// Call the ApplyCurrentValues method to apply changes 
						// from the updated item to the original version. 
						context.ApplyCurrentValues(key.EntitySetName, dataType);
					}
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}