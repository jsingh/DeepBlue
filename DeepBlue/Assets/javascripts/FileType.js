var fileType={
	init: function () {
		this.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditFileType/"+id+"?t="+dt.getTime();
		$("#addInvTypeDialog").remove();
		var iframe=document.createElement("div");
		iframe.id="addInvTypeDialog";
		iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
		iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0"  width="100%" frameborder="0" class="externalSite"  />';
		var ifrm=$("#iframe_modal",iframe).get(0);
		$(ifrm).css("height","100px").unbind('load');
		$(ifrm).load(function () { $("#loading",iframe).remove(); });
		ifrm.src=url;
		$(iframe).dialog({
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
					alert(data);
				} else {
					$("#FileTypeList").flexReload();
				}
			});
		}
	}
	,resizeIframe: function () {
		$("document").ready(function () {
			var theFrame=$("#iframe_modal",parent.document.body);
			if(theFrame) {
				theFrame.height($("body").height());
			}
		});
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId);
	}
	,onRowBound: function (tr,row) {
		$("td:last div",tr).html("<img id='Edit' src='/Assets/images/Edit.gif'/>&nbsp;&nbsp;&nbsp;<img id='Delete' src='/Assets/images/Delete.png'/>");
		$("td:not(:last)",tr).click(function () { fileType.add(row.cell[0]); });
		$("#Edit",tr).click(function () { fileType.add(row.cell[0]); });
		$("#Delete",tr).click(function () { fileType.deleteType(row.cell[0]); });
	}
	,closeDialog: function (reload) {
		$("#addInvTypeDialog").dialog('close');
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
			alert(UpdateTargetId.html())
		} else {
			parent.fileType.closeDialog(true);
		}
	}
}