using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue {
    public enum ConfigUtil {
        //public static int SystemEntityID = 1;
        ///// <summary>
        ///// Each entity should get an ID greater or equal to this value.
        ///// This is used in the validation to make sure we are not inserting any
        ///// data which is for an invalid Entity( 0, or SystemEntity)
        ///// </summary>
        //public static int EntityIDStartRange = 2;
        ///// <summary>
        ///// Data point used to make sure all the IDs start from this range. 
        ///// Used for validation
        ///// </summary>
        //public static int IDStartRange = 1;
        //public static int CurrentEntityID = 2;
        SystemEntityID = 1,
        /// <summary>
        /// Each entity should get an ID greater or equal to this value.
        /// This is used in the validation to make sure we are not inserting any
        /// data which is for an invalid Entity( 0, or SystemEntity)
        /// </summary>
        EntityIDStartRange = 2,
        /// <summary>
        /// Data point used to make sure all the IDs start from this range. 
        /// Used for validation
        /// </summary>
        IDStartRange = 1,
        CurrentEntityID = 2
    }
}