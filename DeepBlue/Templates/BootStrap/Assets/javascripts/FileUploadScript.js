var fileUpload={
	url: ''
	,maxFiles: 100
	,fileName: 'Document'
	,filesize: 50
	,fileExt: [".jpg",".xls",".xlsx"]
	,errorMsg: ''
	,onValid: null
	,onBeforeSend: function () { var data={};return data; }
	,onBrowserNotSuppot: null
	,onCreateBox: null
	,onUploadFinished: function () { return true; }
	,onAfterAll: null
	,onProgress: null
	,onUploadStart: null
	,onFilesAdded: null
	,setDropBox: function (dropbox) {
		message=$('.message',dropbox);
		dropbox.filedrop({
			// The name of the $_FILES entry:
			paramname: fileUpload.fileName,
			maxfiles: fileUpload.maxFiles,
			maxfilesize: fileUpload.filesize,
			url: fileUpload.url,
			//			uploadFinished: function (i,file,response) {
			//				$.data(file).addClass('done');
			//				// response is the JSON object that post_file.php returns
			//			},
			error: function (err,file) {
				switch(err) {
					case 'BrowserNotSupported':
						if(fileUpload.onBrowserNotSuppot) {
							fileUpload.onBrowserNotSuppot();
						}
						//showMessage('Your browser does not support HTML5 file uploads!');
						break;
					case 'TooManyFiles':
						jAlert('Too many files! Please select '+fileUpload.maxFiles+' at most! (configurable)');
						break;
					case 'FileTooLarge':
						jAlert(file.name+' is too large! Please upload files up to '+fileUpload.filesize+'mb (configurable).');
						break;
					default:
						break;
				}
			},
			// Called before each upload is started
			beforeEach: function (file,fileindex) {
				if(!($("#preview_"+fileindex,dropbox).get(0))) {
					return false;
				}
				fileUpload.errorMsg="";
				var i;var msg="";
				for(i=0;i<fileUpload.fileExt.length;i++) {
					msg+=fileUpload.fileExt[i]+" ,";
				}
				msg+=" files only allowed.";
				var notValid=true;
				for(i=0;i<fileUpload.fileExt.length;i++) {
					var filext=file.name.substring(file.name.lastIndexOf(".")+1);
					if(filext.toString().toLowerCase()==fileUpload.fileExt[i].replace(".","")) {
						notValid=false;
					}
				}
				if(notValid) {
					fileUpload.errorMsg=msg;
					$("#preview_"+fileindex).remove();
					jAlert(msg);
					return false;
				}
				//			if(!file.type.match(/^image\//)) {
				//				jAlert('Only images are allowed!');
				//				// Returning false will cause the
				//				// file to be rejected
				//				return false;
				//			}
			},
			beforeSend: function () {
				if(fileUpload.onBeforeSend) {
					return fileUpload.onBeforeSend();
				}
			},
			beforeValid: function () {
				//								if($.trim(fileUpload.errorMsg)!="") {
				//									jAlert(fileUpload.errorMsg);
				//									return false;
				//								}
				if(fileUpload.onValid) {
					return fileUpload.onValid();
				}
			},
			uploadStarted: function (i,file,len) {
				createImage(i,file);
			},
			uploadFinished: function (index,file,jsonData,timeDiff) {
				return fileUpload.onUploadFinished(index,file,jsonData,timeDiff);
			},
			uploadPending: function (uploadStart) {
				fileUpload.onUploadStart=uploadStart;
			},
			afterAll: function () {
				if(fileUpload.onAfterAll) {
					fileUpload.onAfterAll();
				}
			},
			progressUpdated: function (i,file,progress) {
				if(fileUpload.onProgress) {
					fileUpload.onProgress(i,file,progress);
				}
				//$.data(file).find('.progress').width(progress);
			}
		});
		var template='<div class="preview">'+
						'<div class="remove">&nbsp;</div>'+
						'<div class="file-name">'+
						'</div>'+
						'<div class="progressHolder">'+
							'<div class="progress"></div>'+
						'</div>'+
					'</div>';
		function createImage(fileindex,file) {

			var preview=$(template);
			$(preview).attr("id","preview_"+fileindex);
			$(".progress",preview).attr("id","prsbar_"+fileindex);
			$(".file-name",preview).html(file.name);
			$(".remove",preview).click(function () { preview.remove(); });
			//image=$('img',preview);
			//var reader=new FileReader();
			//image.width=100;
			//image.height=100;
			//reader.onload=function (e) {
			// e.target.result holds the DataURL which
			// can be used as a source of the image:
			//image.attr('src',e.target.result);
			//};
			// Reading the file as a DataURL. When finished,
			// this will trigger the onload function above:
			//reader.readAsDataURL(file);
			message.hide();
			if(fileUpload.onCreateBox) {
				fileUpload.onCreateBox(preview);
			}
			// Associating a preview container
			// with the file, using jQuery's $.data():
			$.data(file,preview);
		}
		function showMessage(msg) {
			message.html(msg);
		}
	}
};
//$(function () {
//	var dropbox=$('#dropbox'),
//		message=$('.message',dropbox);

//	jAlert($(dropbox).get(0));

//});