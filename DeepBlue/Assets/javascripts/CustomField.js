var customField={
	init: function () {
		this.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditCustomField/"+id+"?t="+dt.getTime();
		$("#addCustomFieldDialog").remove();
		var iframe=document.createElement("div");
		iframe.id="addCustomFieldDialog";
		iframe.innerHTML+="<div id='loading'><img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...</div>";
		iframe.innerHTML+='<iframe id="iframe_modal" allowtransparency="true" marginheight="0" marginwidth="0"  width="100%" frameborder="0" class="externalSite"  />';
		var ifrm=$("#iframe_modal",iframe).get(0);
		$(ifrm).css("height","100px").unbind('load');
		$(ifrm).load(function () { $("#loading",iframe).remove(); });
		ifrm.src=url;
		$(iframe).dialog({
			title: "Custom Field",
			autoOpen: true,
			width: 400,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteCustomField: function (id,img) {
		if(confirm("Are you sure you want to delete this custom field?")) {
			var dt=new Date();
			var url="/Admin/DeleteCustomField/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#CustomFieldList").flexReload();
				}
			});
		}
	}
	,changeModule: function (moduleddl) {
		var dt=new Date();
		var url="/Admin/CustomFieldTextAvailable/?CustomFieldText="+$("#CustomFieldText").val()+"&CustomFieldId="+$("#CustomFieldId").val()+"&ModuleId="+$("#ModuleId").val()+"&t="+dt.getTime();
		$.get(url,function (data) {
			if(data!="") {
				alert(data);
				$("#CustomFieldText_validationMessage").html(data);
			} else {
				$("#CustomFieldText_validationMessage").html("");
			}
		});
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
		return true;
	}	
	,onRowBound: function (tr,data) {
		var lastcell=$("td:last div",tr);
		lastcell.html("<img id='Edit' src='/Assets/images/Edit.gif'/>&nbsp;&nbsp;&nbsp;<img id='Delete' src='/Assets/images/Delete.png'/>");
		$("#Edit",lastcell).click(function () { customField.add(data.cell[0]); });
		$("#Delete",lastcell).click(function () { customField.deleteCustomField(data.cell[0],this); });
		$("td:not(:last)",tr).click(function () { customField.add(data.cell[0]); });
	}
	,closeDialog: function (reload) {
		$("#addCustomFieldDialog").dialog('close');
		if(reload==true) {
			$("#CustomFieldList").flexReload();
		}
	}
	,onCreateCustomFieldBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateCustomFieldSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			alert(UpdateTargetId.html())
		} else {
			parent.customField.closeDialog(true);
		}
	}
}