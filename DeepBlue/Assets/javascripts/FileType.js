var fileType={
	init: function () {
		jHelper.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditFileType/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: "File Type",
			autoOpen: true,
			width: 400,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteType: function (id,img) {
		if(confirm("Are you sure you want to delete this file type?")) {
			var dt=new Date();
			var url="/Admin/DeleteFileType/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					jAlert(data);
				} else {
					$("#FileTypeList").flexReload();
				}
			});
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId);
	}
	,onRowBound: function (tr,row) {
		$("td:last div",tr).html("<img id='Edit' class='gbutton' src='/Assets/images/Edit.png'/>&nbsp;&nbsp;&nbsp;<img id='Delete' src='/Assets/images/largedel.png'/>");
		$("td:not(:last)",tr).click(function () { fileType.add(row.cell[0]); });
		$("#Edit",tr).click(function () { fileType.add(row.cell[0]); });
		$("#Delete",tr).click(function () { fileType.deleteType(row.cell[0]); });
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#FileTypeList").flexReload();
		}
	}
	,onCreateFileTypeBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateFileTypeSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			jAlert(UpdateTargetId.html())
		} else {
			parent.fileType.closeDialog(true);
		}
	}
}