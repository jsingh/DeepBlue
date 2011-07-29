using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;

namespace DeepBlue.Helpers {
	public class UploadFile {

		public UploadFile(HttpPostedFileBase uploadFile, string appSettingName, params object[] args) {
			_UploadFile = uploadFile;
			_Arguments = args;
			_AppSettingName = appSettingName;
		}

		public UploadFile(string appSettingName) {
			_AppSettingName = appSettingName;
		}


		private object[] _Arguments;

		private string _AppSettingName;

		private FileInfo _FileInfo;

		private HttpPostedFileBase _UploadFile;

		public string FilePath { get; private set; }

		public string FileName { get; private set; }

		public long Size { get; private set; }

		public FileInfo FileInfo {
			get {
				return this._FileInfo;
			}
		}

		public bool Upload() {
			string rootPath = HttpContext.Current.Server.MapPath("/");
			string uploadFilePath = Path.Combine(rootPath, string.Format(ConfigurationManager.AppSettings[_AppSettingName], _Arguments));
			string directoryName = Path.GetDirectoryName(uploadFilePath);
			if (Directory.Exists(directoryName) == false) {
				Directory.CreateDirectory(directoryName);
			}
			if (File.Exists(uploadFilePath)) {
				File.Delete(uploadFilePath);
			}
			_UploadFile.SaveAs(uploadFilePath);
			_FileInfo = new FileInfo(uploadFilePath);
			FileName = _FileInfo.Name;
			FilePath = directoryName.Replace(rootPath, "");
			Size = _FileInfo.Length;
			return true;
		}


		public bool Move(string tempFileName, params object[] args) {
			if (File.Exists(tempFileName)) {
				FileInfo tempFileInfo = new FileInfo(tempFileName);
				string rootPath = HttpContext.Current.Server.MapPath("/");
				string uploadFilePath = Path.Combine(rootPath, string.Format(ConfigurationManager.AppSettings[_AppSettingName], args));
				string directoryName = Path.GetDirectoryName(uploadFilePath);
				if (Directory.Exists(directoryName) == false) {
					Directory.CreateDirectory(directoryName);
				}
				if (File.Exists(uploadFilePath)) {
					File.Delete(uploadFilePath);
				}
				if (File.Exists(tempFileName)) {
					File.Move(tempFileName, uploadFilePath);
				}
				_FileInfo = new FileInfo(uploadFilePath);
				FileName = _FileInfo.Name;
				FilePath = directoryName.Replace(rootPath, "");
				Size = _FileInfo.Length;
				return true;
			}
			else {
				return false;
			}
		}

	}
}