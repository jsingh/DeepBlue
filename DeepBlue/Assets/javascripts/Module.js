var module={
	pageLoad: false
	,init: function () {
		$("document").ready(function () {
			module.resizeIframe();
			module.pageLoad=true;
			$("body").css("overflow","hidden");
		});
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditModule/"+id+"?t="+dt.getTime();
		$("#addModuleDialog").remove();
		var iframe=document.createElement("div");
		iframe.id="addModuleDialog";
		iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
		iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0"  width="100%" frameborder="0" class="externalSite"  />';
		var ifrm=$("#iframe_modal",iframe).get(0);
		$(ifrm).css("height","100px").css("overflow","hidden").unbind('load');
		$(ifrm).load(function () { $("#loading",iframe).remove(); });
		ifrm.src=url;
		$(iframe).dialog({
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
					alert(data);
				} else {
					$("#ModuleList").flexReload();
				}
			});
		}
	}
	,resizeIframe: function (h) {
		var theFrame=$("#iframe_modal",parent.document.body);
		if(theFrame) {
			var bdyHeight=$("body").height();
			if(parseInt(h)>0&&this.pageLoad) {
				bdyHeight=bdyHeight+h;
			}
			theFrame.height(bdyHeight);
		}
	}
	,onSubmit: function (formId) {
		
		try {
			var frm=document.getElementById(formId);
			var message='';
			$(".field-validation-error",frm).each(function () {
				if(this.innerHTML!='') {
					message+=this.innerHTML+"\n";
				}
			});
			if(message!="") {
				alert(message);
				return false;
			}
			Sys.Mvc.FormContext.getValidationForForm(frm).validate('submit');
			$(".field-validation-error",frm).each(function () {
				if(this.innerHTML!='') {
					message+=this.innerHTML+"\n";
				}
			});
			if(message!="") {
				alert(message);
				return false;
			} else {
				return true;
			}
		} catch(e) {
		}
		return true;
	}
	,closeDialog: function (reload) {
		$("#addModuleDialog").dialog('close');
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
			alert(UpdateTargetId.html())
		} else {
			parent.module.closeDialog(true);
		}
	}
}