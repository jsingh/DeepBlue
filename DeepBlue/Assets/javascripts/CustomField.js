var customField={
	init: function () {
		jHelper.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditCustomField/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
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
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId);
	}
	,onRowBound: function (tr,data) {
		var lastcell=$("td:last div",tr);
		lastcell.html("<img id='Edit' class='gbutton' src='/Assets/images/Edit.png'/>&nbsp;&nbsp;&nbsp;<img id='Delete' class='gbutton' src='/Assets/images/largedel.png'/>");
		$("#Edit",lastcell).click(function () { customField.add(data.cell[0]); });
		$("#Delete",lastcell).click(function () { customField.deleteCustomField(data.cell[0],this); });
		$("td:not(:last)",tr).click(function () { customField.add(data.cell[0]); });
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
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