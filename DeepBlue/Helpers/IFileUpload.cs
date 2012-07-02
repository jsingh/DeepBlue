using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DeepBlue.Models.File;
using System.IO;

namespace DeepBlue.Helpers {
	interface IFileUpload {
		UploadPathKeyCollection UploadPathKeys { get; }
		UploadFileModel UploadFile(HttpPostedFileBase uploadFile,string appSettingName,params object[] args);
		UploadFileModel UploadTempFile(HttpPostedFileBase uploadFile);
		bool DeleteFile(Models.Entity.File file);
		FileInfo WriteTempFileText(string fileName,string contents);
		bool TempFileExist(string fileName);
		bool TempFileDelete(string fileName);
		FileInfo TempFileWriteAllBytes(string fileName,byte[] bytes);
	}
}
