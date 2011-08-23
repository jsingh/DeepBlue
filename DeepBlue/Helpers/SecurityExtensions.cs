using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace DeepBlue.Helpers {
	public static class SecurityExtensions {

		public static bool ComparePassword(this string source, string salt, string value) {
			return value.CreateHash(salt) == source;
		}

		public static string CreateHash(this string source, string salt) {

			HMACSHA1 hmacsha1 = new HMACSHA1();
			hmacsha1.Key = ASCIIEncoding.ASCII.GetBytes(SecurityKey);
			hmacsha1.ComputeHash(source.ConvertToByte(salt));

			return ASCIIEncoding.ASCII.GetString(hmacsha1.Hash);
		}

		public static string CreateSalt() {
			//Generate a cryptographic random number.
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			byte[] buff = new byte[24];
			rng.GetBytes(buff);

			// Return a Base64 string representation of the random number.
			return Convert.ToBase64String(buff);
		}


		#region Private Properties

		private static string SecurityKey {
			get {
				// Create a new appsettings named Password.Key in the web.config 
				// Set its value to 7AB3996B-EF9C-4835-8D0A-59B10B4E0150
				// Read that appsettings key and return the value
				return ConfigurationManager.AppSettings["Password.Key"];
			}
		}
		#endregion Private Properties

		#region Private Methods

		private static byte[] ConvertToByte(this string source, string salt) {
			return ASCIIEncoding.ASCII.GetBytes(source + salt);
		}
		#endregion Private Methods
	}
}