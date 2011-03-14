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

		private object[] _Arguments;

		private string _AppSettingName;

		private HttpPostedFileBase _UploadFile;

		public string FilePath { get; private set; }

		public string FileName { get; private set; }

		public long Size { get; private set; }

		public bool Upload() {
			string rootPath = HttpContext.Current.Server.MapPath("/");
			string uploadFilePath = Path.Combine(rootPath, string.Format(ConfigurationManager.AppSettings[_AppSettingName], _Arguments));
			string directoryName = Path.GetDirectoryName(uploadFilePath);
			if (Directory.Exists(directoryName)==false) {
				Directory.CreateDirectory(directoryName);
			}
			_UploadFile.SaveAs(uploadFilePath);
			FileInfo fileInfo = new FileInfo(uploadFilePath);
			FileName = fileInfo.Name;
			FilePath = directoryName.Replace(rootPath, "");
			Size = fileInfo.Length;
			return true;
		}
	}
}