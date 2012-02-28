using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;

namespace DeepBlue.Helpers {
	public class FileHelper {

		public static bool CheckFilePath(string filePath) {
			Regex regex = new Regex(
									@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)"
									+ @"*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$",
									RegexOptions.IgnoreCase
									| RegexOptions.Multiline
									| RegexOptions.IgnorePatternWhitespace
									| RegexOptions.Compiled
									);
			return regex.IsMatch(filePath);
		}

		public static Models.Entity.FileType CheckFileExtension(List<Models.Entity.FileType> fileTypes, string extension, out string errorMessage) {
			Models.Entity.FileType fileType = null;
			errorMessage = "*.";
			foreach (var type in fileTypes) {
				errorMessage += type.FileExtension + ",";
				if (fileType == null) {
					var arrExtensions = type.FileExtension.Split((",").ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					foreach (var ext in arrExtensions) {
						if (ext.Replace(".", "").ToLower() == extension.Replace(".", "").ToLower()) {
							fileType = type;
							break;
						}
					}
				}
				else {
					break;
				}
			}
			if (fileType == null) {
				errorMessage += "  files only allowed";
			}
			return fileType;
		}

		public static void MoveFile(string sourceFileName, string destinationFileName, bool removeSourceDirectory) {
			if (File.Exists(sourceFileName)) {
				string directoryName = Path.GetDirectoryName(destinationFileName);
				if (Directory.Exists(directoryName) == false) {
					Directory.CreateDirectory(directoryName);
				}
				File.Move(sourceFileName, destinationFileName);
				if (removeSourceDirectory) {
					Directory.Delete(directoryName, true);
				}
			}
		}


		public static void DeleteFile(Models.Entity.File file) {
			try {
				if (file != null) {
					string rootPath = HttpContext.Current.Server.MapPath("~/");
					string fileName = Path.Combine(rootPath, file.FilePath, file.FileName);
					if (File.Exists(fileName)) {
						File.Delete(fileName);
					}
				}
			}
			catch { }
		}
	}
}