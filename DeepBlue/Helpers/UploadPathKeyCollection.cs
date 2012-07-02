using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace DeepBlue.Helpers {
 /// <summary>
    /// The collection class that will store the list of each element/item that
    /// is returned back from the configuration manager.
    /// </summary>
    [ConfigurationCollection(typeof(UploadPathElement))]
    public class UploadPathKeyCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UploadPathElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UploadPathElement)(element)).Key;
        }

        public UploadPathElement this[int idx]
        {
            get
            {
                return (UploadPathElement)BaseGet(idx);
            }
        }

		    public UploadPathElement this[string key]
        {
            get
            {
                return (UploadPathElement)BaseGet(key);
            }
        }
    }
}