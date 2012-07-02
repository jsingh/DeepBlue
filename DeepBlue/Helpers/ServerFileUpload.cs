using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using DeepBlue.Models.File;

namespace DeepBlue.Helpers {

	public class ServerFileUpload:ConfigurationSection,IFileUpload {

		[ConfigurationProperty("UploadPathKeys")]
		public UploadPathKeyCollection UploadPathKeys {
			get { return ((UploadPathKeyCollection)(base["UploadPathKeys"])); }
		}

		public bool DeleteFile(Models.Entity.File file) {
			bool result=false;
			if(file!=null) {
				string rootPath=HttpContext.Current.Server.MapPath("~/");
				string fileName=Path.Combine(rootPath,file.FilePath,file.FileName);
				if(File.Exists(fileName)) {
					File.Delete(fileName);
					result=true;
				}
			}
			return result;
		}

		public UploadFileModel UploadFile(HttpPostedFileBase uploadFile,string appSettingName,params object[] args) {
			string rootPath=HttpContext.Current.Server.MapPath("~/");
			string uploadFilePath=Path.Combine(rootPath,string.Format(this.UploadPathKeys[appSettingName].Value,args));
			string directoryName=Path.GetDirectoryName(uploadFilePath);
			UploadFileModel uploadFileModel=null;
			if(Directory.Exists(directoryName)==false) {
				Directory.CreateDirectory(directoryName);
			}
			if(File.Exists(uploadFilePath)) {
				File.Delete(uploadFilePath);
			}
			uploadFile.SaveAs(uploadFilePath);
			FileInfo fileInfo=new FileInfo(uploadFilePath);
			uploadFileModel=new UploadFileModel {
				FileName=fileInfo.Name,
				FilePath=directoryName.Replace(rootPath,""),
				Size=fileInfo.Length
			};
			return uploadFileModel;
		}

		public UploadFileModel UploadTempFile(HttpPostedFileBase uploadFile) {
			UploadFileModel uploadFileModel=null;
			if(uploadFile!=null) {
				string fileName=uploadFile.FileName;
				if(string.IsNullOrEmpty(fileName)==false) {
					string rootPath=HttpContext.Current.Server.MapPath("~/");
					string tempFileName=Path.Combine(rootPath,string.Format(this.UploadPathKeys["TempUploadPath"].Value,fileName));
					string directoryName=Path.GetDirectoryName(tempFileName);
					if(Directory.Exists(directoryName)==false) {
						Directory.CreateDirectory(directoryName);
					}
					uploadFile.SaveAs(tempFileName);
					FileInfo fileInfo=new FileInfo(tempFileName);
					uploadFileModel=new UploadFileModel {
						FileName=fileInfo.Name,
						FilePath=directoryName,
						Size=fileInfo.Length
					};
				}
			}
			return uploadFileModel;
		}

		public FileInfo WriteTempFileText(string fileName,string contents) {
			string rootPath=HttpContext.Current.Server.MapPath("~/");
			string tempFileName=Path.Combine(rootPath,fileName);
			string directoryName=Path.GetDirectoryName(tempFileName);
			if(Directory.Exists(directoryName)==false) {
				Directory.CreateDirectory(directoryName);
			}
			File.WriteAllText(tempFileName,contents);
			return new FileInfo(tempFileName);
		}

		public bool TempFileExist(string fileName) {
			string rootPath=HttpContext.Current.Server.MapPath("~/");
			string tempFileName=Path.Combine(rootPath,fileName);
			return File.Exists(tempFileName);
		}

		public bool TempFileDelete(string fileName) {
			string rootPath=HttpContext.Current.Server.MapPath("~/");
			string deleteFileName=Path.Combine(rootPath,fileName);
			bool result=false;
			if(File.Exists(deleteFileName)) {
				File.Delete(deleteFileName);
				result=true;
			}
			return result;
		}

		public FileInfo TempFileWriteAllBytes(string fileName,byte[] bytes) {
			string rootPath=HttpContext.Current.Server.MapPath("~/");
			string tempFileName=Path.Combine(rootPath,fileName);
			string directoryName=Path.GetDirectoryName(tempFileName);
			if(Directory.Exists(directoryName)==false) {
				Directory.CreateDirectory(directoryName);
			}
			File.WriteAllBytes(tempFileName,bytes);
			return new FileInfo(tempFileName);
		}

	}

}