var module={
	init: function () {
		jHelper.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditModule/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: " Module ",
			autoOpen: true,
			width: 380,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteModule: function (id,img) {
		if(confirm("Are you sure you want to delete this Module?")) {
			var dt=new Date();
			var url="/Admin/DeleteModule/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					jAlert(data);
				} else {
					$("#ModuleList").flexReload();
				}
			});
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId);
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#ModuleList").flexReload();
		}
	}
	,onModuleBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onModuleSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			jAlert(UpdateTargetId.html())
		} else {
			parent.module.closeDialog(true);
		}
	}
}