using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Text.RegularExpressions;
using DeepBlue.Models.File;

namespace DeepBlue.Helpers {

	public static class UploadFileHelper {

		private static IFileUpload _FileUpload= null;

		static UploadFileHelper(){
			string windowsazure = ConfigurationManager.AppSettings["WindowsAzure"];
			if(windowsazure=="true")
				_FileUpload = (IFileUpload)ConfigurationManager.GetSection("WindowsAzureFileUpload");
			else
				_FileUpload = (IFileUpload)ConfigurationManager.GetSection("ServerFileUpload");
		}

		public static UploadFileModel Upload(HttpPostedFileBase uploadFile,string appSettingName,params object[] args) {
			return _FileUpload.UploadFile(uploadFile,appSettingName,args);
		}

		public static UploadFileModel UploadTempFile(HttpPostedFileBase uploadFile) {
			return _FileUpload.UploadTempFile(uploadFile);
		}

		public static bool DeleteFile(Models.Entity.File file) {
			return _FileUpload.DeleteFile(file);
		}

		public static FileInfo WriteTempFileText(string fileName,string contents) {
			return _FileUpload.WriteTempFileText(fileName,contents);
		}

		public  static bool TempFileExist(string fileName) {
			return _FileUpload.TempFileExist(fileName);
		}

		public static bool TempFileDelete(string fileName) {
			return _FileUpload.TempFileDelete(fileName);
		}

		public static FileInfo TempFileWriteAllBytes(string fileName,byte[] bytes){
			return _FileUpload.TempFileWriteAllBytes(fileName, bytes);
		}

		public static bool CheckFilePath(string filePath) {
			Regex regex=new Regex(
									@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)"
									+@"*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$",
									RegexOptions.IgnoreCase
									|RegexOptions.Multiline
									|RegexOptions.IgnorePatternWhitespace
									|RegexOptions.Compiled
									);
			return regex.IsMatch(filePath);
		}

		public static Models.Entity.FileType CheckFileExtension(List<Models.Entity.FileType> fileTypes,string extension,out string errorMessage) {
			Models.Entity.FileType fileType=null;
			errorMessage="*.";
			foreach(var type in fileTypes) {
				errorMessage+=type.FileExtension+",";
				if(fileType==null) {
					var arrExtensions=type.FileExtension.Split((",").ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
					foreach(var ext in arrExtensions) {
						if(ext.Replace(".","").ToLower()==extension.Replace(".","").ToLower()) {
							fileType=type;
							break;
						}
					}
				} else {
					break;
				}
			}
			if(fileType==null) {
				errorMessage+="  files only allowed";
			}
			return fileType;
		}


		public static string AppSetting(string key) {
			return _FileUpload.UploadPathKeys[key].Value;
		}

	}
}