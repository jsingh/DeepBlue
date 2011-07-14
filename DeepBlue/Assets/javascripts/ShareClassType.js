var shareClassType={
	init: function () {
		jHelper.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditShareClassType/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: "Share Class Type",
			autoOpen: true,
			width: 380,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteShareClassType: function (id,img) {
		if(confirm("Are you sure you want to delete this Share Class Type?")) {
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
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId);
	}
	,onGridSuccess: function (t) {
		$("tr",t).each(function () {
			$("td:last div",this).html("<img id='Edit' class='gbutton' src='/Assets/images/Edit.png'/>");
		});
	}
	,onRowClick: function (row) {
		shareClassType.add(row.cell[0]);
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
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
			parent.shareClassType.closeDialog(true);
		}
	}
}