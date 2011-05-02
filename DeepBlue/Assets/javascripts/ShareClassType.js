var shareclasstype={
	pageLoad: false
	,init: function () {
		$("document").ready(function () {
			shareclasstype.resizeIframe();
			shareclasstype.pageLoad=true;
			$("body").css("overflow","hidden");
		});
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditShareClassType/"+id+"?t="+dt.getTime();
		$("#addShareClassTypeDialog").remove();
		var iframe=document.createElement("div");
		iframe.id="addShareClassTypeDialog";
		iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
		iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0"  width="100%" frameborder="0" class="externalSite"  />';
		var ifrm=$("#iframe_modal",iframe).get(0);
		$(ifrm).css("height","100px").css("overflow","hidden").unbind('load');
		$(ifrm).load(function () { $("#loading",iframe).remove(); });
		ifrm.src=url;
		$(iframe).dialog({
			title: " ShareClassType ",
			autoOpen: true,
			width: 380,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteShareClassType: function (id,img) {
		if(confirm("Are you sure you want to delete this ShareClassType?")) {
			var dt=new Date();
			var url="/Admin/DeleteShareClassType/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#ShareClassTypeList").flexReload();
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
	,onGridSuccess: function (t) {
		$("tr",t).each(function () {
			$("td:last div",this).html("<img id='Edit' src='/Assets/images/Edit.gif'/>");
		});
	}
	,onRowClick: function (row) {
		purchaseType.add(row.cell[0]);
	}
	,closeDialog: function (reload) {
		$("#addShareClassTypeDialog").dialog('close');
		if(reload==true) {
			$("#ShareClassTypeList").flexReload();
		}
	}
	,onShareClassTypeBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onShareClassTypeSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			alert(UpdateTargetId.html())
		} else {
			parent.shareclasstype.closeDialog(true);
		}
	}
}