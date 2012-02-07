var activityType={
	init: function () {
		jHelper.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditActivityType/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: "Activity Type",
			autoOpen: true,
			width: 400,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteType: function (id,img) {
		if(confirm("Are you sure you want to delete this activity type?")) {
			var dt=new Date();
			var url="/Admin/DeleteActivityType/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					jAlert(data);
				} else {
					$("#ActivityTypeList").flexReload();
				}
			});
		}
	}
	,onGridSuccess: function (t) {
		$("tr",t).each(function () {
			$("td:last div",this).html("<img id='Edit' class='gbutton' src='"+jHelper.getImagePath("Edit.png")+"'/>");
		});
	}
	,onRowClick: function (row) {
		activityType.add(row.cell[0]);
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#ActivityTypeList").flexReload();
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId);
	}
	,onCreateBegin: function () {
		$("#UpdateLoading").html(jHelper.savingHTML());
	}
	,onCreateSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			jAlert(UpdateTargetId.html())
		} else {
			parent.activityType.closeDialog(true);
		}
	}
}