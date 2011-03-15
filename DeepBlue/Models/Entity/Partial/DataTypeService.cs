using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
					context.DataTypes.SingleOrDefault(entityType => entityType.DataTypeID == dataType.DataTypeID);
					context.DataTypes.ApplyCurrentValues(dataType);
				}
				context.SaveChanges();
			}
		}

		#endregion
	}
}