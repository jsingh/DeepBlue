using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;
using DeepBlue.Models.File;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace DeepBlue.Helpers {
	public class WindowsAzureFileUpload:ConfigurationSection,IFileUpload {

		private string ConnectionString {
			get {
				return this.StorageConfig["ConnectionString"].Value;
			}
		}

		private string BlogContacinerName {
			get {
				return this.StorageConfig["BlogContainerName"].Value;
			}
		}

		private string LocalStorageName {
			get {
				return this.StorageConfig["LocalStorageName"].Value;
			}
		}

		private CloudBlobContainer GetBlobContainer() {
			// start windows azure file upload.
			// Variables for the cloud storage objects.
			CloudStorageAccount cloudStorageAccount;
			CloudBlobClient blobClient;
			CloudBlobContainer blobContainer;
			BlobContainerPermissions containerPermissions;

			// Use the emulatedstorage account.
			cloudStorageAccount=CloudStorageAccount.DevelopmentStorageAccount;

			// If you want to use Windows Azure cloud storage account, use the following
			// code (after uncommenting) instead of the code above.
			cloudStorageAccount=CloudStorageAccount.Parse(this.ConnectionString);

			// Create the blob client, which provides
			// authenticated access to the Blob service.
			blobClient=cloudStorageAccount.CreateCloudBlobClient();

			// Get the container reference.
			blobContainer=blobClient.GetContainerReference(this.BlogContacinerName);
			// Create the container if it does not exist.
			blobContainer.CreateIfNotExist();

			// Set permissions on the container.
			containerPermissions=new BlobContainerPermissions();
			// This sample sets the container to have public blobs. Your application
			// needs may be different. See the documentation for BlobContainerPermissions
			// for more information about blob container permissions.
			containerPermissions.PublicAccess=BlobContainerPublicAccessType.Blob;
			blobContainer.SetPermissions(containerPermissions);

			return blobContainer;
		}

		[ConfigurationProperty("StorageConfig")]
		public UploadPathKeyCollection StorageConfig {
			get { return ((UploadPathKeyCollection)(base["StorageConfig"])); }
		}


		[ConfigurationProperty("UploadPathKeys")]
		public UploadPathKeyCollection UploadPathKeys {
			get { return ((UploadPathKeyCollection)(base["UploadPathKeys"])); }
		}

		public Models.File.UploadFileModel UploadFile(HttpPostedFileBase uploadFile,string appSettingName,params object[] args) {
			UploadFileModel uploadFileModel=null;
			string fileName=string.Empty;
			if(uploadFile!=null) {
				fileName=uploadFile.FileName;
			}
			if(string.IsNullOrEmpty(fileName)==false) {
				string uploadFileName=string.Format(this.UploadPathKeys[appSettingName].Value,args);

				CloudBlobContainer blobContainer=GetBlobContainer();
				CloudBlob blob=blobContainer.GetBlobReference(uploadFileName);
				blob.DeleteIfExists();

				// Upload a file from the local system to the blob.
				blob.UploadFromStream(uploadFile.InputStream);  // File from emulated storage.

				string blobURL=blob.Uri.ToString();
				string blobFileName=Path.GetFileName(blobURL);
				blobURL=blobURL.Replace(blobFileName,"");
				uploadFileModel=new UploadFileModel { FileName=blobFileName,FilePath=blobURL,Size=uploadFile.ContentLength };
			}
			return uploadFileModel;

		}

		public Models.File.UploadFileModel UploadTempFile(HttpPostedFileBase uploadFile) {
			UploadFileModel uploadFileModel=null;
			if(uploadFile!=null) {
				string fileName=uploadFile.FileName;
				if(string.IsNullOrEmpty(fileName)==false) {
					LocalResource localResource=RoleEnvironment.GetLocalResource(this.LocalStorageName);
					string rootPath=localResource.RootPath;
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

		public bool DeleteFile(Models.Entity.File file) {
			bool result=false;
			if(file!=null) {
				CloudBlobContainer blobContainer=GetBlobContainer();
				CloudBlob blob=blobContainer.GetBlobReference(file.FileName);
				blob.DeleteIfExists();
			}
			return result;
		}

		public System.IO.FileInfo WriteTempFileText(string fileName,string contents) {
			LocalResource localResource=RoleEnvironment.GetLocalResource(this.LocalStorageName);
			string rootPath=localResource.RootPath;
			string tempFileName=Path.Combine(rootPath,fileName);
			string directoryName=Path.GetDirectoryName(tempFileName);
			if(Directory.Exists(directoryName)==false) {
				Directory.CreateDirectory(directoryName);
			}
			File.WriteAllText(tempFileName,contents);
			return new FileInfo(tempFileName);
		}

		public bool TempFileExist(string fileName) {
			LocalResource localResource=RoleEnvironment.GetLocalResource(this.LocalStorageName);
			string rootPath=localResource.RootPath;
			string tempFileName=Path.Combine(rootPath,fileName);
			return File.Exists(tempFileName);
		}

		public bool TempFileDelete(string fileName) {
			LocalResource localResource=RoleEnvironment.GetLocalResource(this.LocalStorageName);
			string rootPath=localResource.RootPath;
			string deleteFileName=Path.Combine(rootPath,fileName);
			bool result=false;
			if(File.Exists(deleteFileName)) {
				File.Delete(deleteFileName);
				result=true;
			}
			return result;
		}

		public System.IO.FileInfo TempFileWriteAllBytes(string fileName,byte[] bytes) {
			LocalResource localResource=RoleEnvironment.GetLocalResource(this.LocalStorageName);
			string rootPath=localResource.RootPath;
			string tempFileName=Path.Combine(rootPath,fileName);
			string directoryName=Path.GetDirectoryName(tempFileName);
			if(Directory.Exists(directoryName)==false) {
				Directory.CreateDirectory(directoryName);
			}
			File.WriteAllBytes(tempFileName,bytes);
			return new FileInfo(tempFileName);
		}
	}

	[ConfigurationCollection(typeof(StorageConfigElement))]
	public class StorageConfigCollection:ConfigurationElementCollection {
		protected override ConfigurationElement CreateNewElement() {
			return new StorageConfigElement();
		}

		protected override object GetElementKey(ConfigurationElement element) {
			return ((StorageConfigElement)(element)).Key;
		}

		public StorageConfigElement this[int idx] {
			get {
				return (StorageConfigElement)BaseGet(idx);
			}
		}

		public StorageConfigElement this[string key] {
			get {
				return (StorageConfigElement)BaseGet(key);
			}
		}
	}

	/// <summary>
	/// The class that holds onto each element returned by the configuration manager.
	/// </summary>
	public class StorageConfigElement:ConfigurationElement {
		[ConfigurationProperty("key",DefaultValue="",IsKey=true,IsRequired=true)]
		public string Key {
			get {
				return ((string)(base["key"]));
			}
			set {
				base["key"]=value;
			}
		}

		[ConfigurationProperty("value",DefaultValue="",IsKey=false,IsRequired=false)]
		public string Value {
			get {
				return ((string)(base["value"]));
			}
			set {
				base["value"]=value;
			}
		}
	}
}
