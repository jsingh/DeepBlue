using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace DeepBlue.ImportData {
    public static class Extensions {
        public static NameValueCollection Combine(this NameValueCollection target, NameValueCollection source) {
            foreach (string key in source.Keys) {
                if (target.AllKeys.Contains(key)) {
                    string value = source[key];
                    if (!string.IsNullOrEmpty(value)) {
                        string targetVal = target[key];
                        string newValue = value;
                        if (!string.IsNullOrEmpty(targetVal)) {
                            newValue = targetVal + "," + value;
                        }
                    }
                } else {
                    target.Add(key, source[key]);
                }
            }
            return target;
        }
    }
}
