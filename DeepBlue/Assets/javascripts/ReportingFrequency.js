﻿var reportingFrequency={
	init: function () {
		jHelper.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditReportingFrequency/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: "Reporting Frequency",
			autoOpen: true,
			width: 380,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteReportingFrequency: function (id,img) {
		if(confirm("Are you sure you want to delete this Reporting Frequency?")) {
			var dt=new Date();
			var url="/Admin/DeleteReportingFrequency/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#ReportingFrequencyList").flexReload();
				}
			});
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId);
	}
	,onRowBound: function (tr,row) {
		$("td:last div",tr).html("<img id='Edit' src='/Assets/images/Edit.png'/>&nbsp;&nbsp;&nbsp;<img id='Delete' src='/Assets/images/largedel.png'/>");
		$("td:not(:last)",tr).click(function () { reportingFrequency.add(row.cell[0]); });
		$("#Edit",tr).click(function () { reportingFrequency.add(row.cell[0]); });
		$("#Delete",tr).click(function () { reportingFrequency.deleteReportingFrequency(row.cell[0]); });
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#ReportingFrequencyList").flexReload();
		}
	}
	,onReportingFrequencyBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onReportingFrequencySuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			alert(UpdateTargetId.html())
		} else {
			parent.reportingFrequency.closeDialog(true);
		}
	}
}