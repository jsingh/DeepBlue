$(document).ready(function () {
	var target=$("#ExcelImport");
	target.dialog({
		title: "Import Underlying Fund Valuation",
		autoOpen: false,
		width: 400,
		modal: true,
		position: 'middle',
		autoResize: true,
		open: function () {
			var data=[{ name: ""}];
			target.empty();
			$("#ExcelImprtTemplate").tmpl(data).appendTo(target);
			//importExcel.init();
		}
	});
});
var importExcel={
	defaultID: "DropFileUpload",
	init: function () {
		dropZone=$("#"+importExcel.defaultID);
		dropZone.removeClass('error');
		// Check if window.FileReader exists to make 
		// sure the browser supports file uploads
		if(typeof (window.FileReader)=='undefined') {
			dropZone.text('Browser Not Supported!');
			dropZone.addClass('error');
			return;
		}
		// Add a nice drag effect
		dropZone[0].ondragover=function () {
			dropZone.addClass('hover');
			return false;
		};
		// Remove the drag effect when stopping our drag
		dropZone[0].ondragend=function () {
			dropZone.removeClass('hover');
			return false;
		};
		// The drop event handles the file sending
		dropZone[0].ondrop=function (event) {
			// Stop the browser from opening the file in the window
			event.preventDefault();
			dropZone.removeClass('hover');
			// Get the file and the file reader
			var file=event.dataTransfer.files[0];
			// Validate file size
			if(file.size>1650065408) {
				dropZone.text('File Too Large!');
				dropZone.addClass('error');
				return false;
			}
			var randomnumber=Math.floor(Math.random()*100001)
			var fileName=randomnumber+"_"+file.name.replace(" ","_");
			// Send the file
			var xhr=new XMLHttpRequest();
			xhr.upload.addEventListener('progress',function (event) {
				var percent=parseInt(event.loaded/event.total*100);
				dropZone.text('Uploading: '+percent+'%');
			},false);
			xhr.onreadystatechange=function (event) {
				if(event.target.readyState==4) {
					if(event.target.status==200||event.target.status==304) {
						dropZone.text('Upload Complete!');
						importExcel.importRows(fileName);
					}
					else {
						dropZone.text('Upload Failed!');
						dropZone.addClass('error');
					}
				}
			};
			xhr.open('POST','/Admin/Upload',true);
			xhr.setRequestHeader('UPLOAD-FILE-NAME',fileName);
			xhr.send(file);
		};
	}
	,uploadExcel: function () {
		try {
			var loading=$("#SpnUELoading");
			loading.html(jHelper.uploadingHTML());
			$.ajaxFileUpload(
			{
				url: '/Admin/Upload',
				secureuri: false,
				formId: 'frmUploadExcel',
				dataType: 'json',
				success: function (data,status) {
					loading.empty();
					if($.trim(data.Result)!="") {
						jAlert(data.Result);
					} else {
						importExcel.importRows(data.FileName);
					}
				},
				error: function (data,status,e) {
					jAlert(data.msg+","+status+","+e);
				}
			}
		);
		} catch(e) {
			jAlert(e);
		}
	}
	,importRows: function (fileName) {
		var importBox=$(".import-box","#ExcelImport");
		importBox.hide();
		var param=[{ name: "FileName",value: fileName}];
		var target=$("#ImportUFV","#ExcelImport");
		target.empty();
		target.html("<center>"+jHelper.loadingHTML()+"</center>");
		$.post("/Deal/ImportExcel",param,function (data) {
			if($.trim(data.Result)!="") {
				jAlert(data.Result);
				importBox.show();
				target.empty();
			} else {
				target.empty();
				$.each(data.Tables,function (i,item) {
					$("#ImportUFVTemplate").tmpl(item).appendTo(target);
					$("select",target).each(function () {
						var ddl=this;
						ddl.options.length=null;
						var listItem=new Option("--Select One--"," ",false,false);
						ddl.options[ddl.options.length]=listItem;
						$.each(item.Columns,function (i,name) {
							if(name!="RowNumber") {
								listItem=new Option(name,name,false,false);
								ddl.options[ddl.options.length]=listItem;
							}
						});
						ddl.value=ddl.name;
					});
					jHelper.jqComboBox(target);
					return false;
				});
			}
		},"JSON");
	}
	,importUnderlyingFund: function (formId,pageIndex) {
		var frm=$("#"+formId);
		var target=$("#ProgressBar","#ExcelImport");
		var ImportUFV=$("#ImportUFV");
		var param=$(frm).serializeForm();
		param[param.length]={ name: "PageIndex",value: parseInt(pageIndex)+1 };
		$.post("/Deal/ImportUnderlyingFundValuation",param,function (data) {
			if($.trim(data.Result)!="") {
				jAlert(data.Result);
			} else {
				frm.hide();
				if(pageIndex==0) {
					target.empty();
					var data=[{ TotalRows: $("#TotalRows",frm).val(),CompletedRows: 0,Percent: 0}];
					$("#ProgressBarTemplate").tmpl(data).appendTo(target);
				}
				target.empty();
				data.Percent=parseInt((data.CompletedRows/data.TotalRows)*100);
				$("#ProgressBarTemplate").tmpl(data).appendTo(target);
				if(data.TotalPages>=(data.PageIndex+1)) {
					importExcel.importUnderlyingFund(formId,data.PageIndex);
				} else {
					$('#ExcelImport').dialog('close');
				}
			}
		},"JSON");
	}
}