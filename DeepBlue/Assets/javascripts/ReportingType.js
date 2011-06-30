var reportingType={
	init: function () {
		jHelper.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditReportingType/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: "Reporting Type",
			autoOpen: true,
			width: 380,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteReportingType: function (id,img) {
		if(confirm("Are you sure you want to delete this Reporting Type?")) {
			var dt=new Date();
			var url="/Admin/DeleteReportingType/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#ReportingTypeList").flexReload();
				}
			});
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId);
	}
	,onRowBound: function (tr,row) {
		$("td:last div",tr).html("<img id='Edit' src='/Assets/images/Edit.png'/>&nbsp;&nbsp;&nbsp;<img id='Delete' src='/Assets/images/largedel.png'/>");
		$("td:not(:last)",tr).click(function () { reportingType.add(row.cell[0]); });
		$("#Edit",tr).click(function () { reportingType.add(row.cell[0]); });
		$("#Delete",tr).click(function () { reportingType.deleteReportingType(row.cell[0]); });
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#ReportingTypeList").flexReload();
		}
	}
	,onReportingTypeBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onReportingTypeSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			alert(UpdateTargetId.html())
		} else {
			parent.reportingType.closeDialog(true);
		}
	}
}