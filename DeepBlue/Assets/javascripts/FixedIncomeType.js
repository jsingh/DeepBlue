﻿var fixedIncomeType={
	init: function () {
		jHelper.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditFixedIncomeType/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: "FixedIncome Type",
			autoOpen: true,
			width: 400,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteType: function (id,img) {
		if(confirm("Are you sure you want to delete this fixedIncome type?")) {
			var dt=new Date();
			var url="/Admin/DeleteFixedIncomeType/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					jAlert(data);
				} else {
					$("#FixedIncomeTypeList").flexReload();
				}
			});
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId);
	}
	,onGridSuccess: function (t) {
		$("tr",t).each(function () {
			$("td:last div",this).html("<img id='Edit' class='gbutton' src='"+jHelper.getImagePath("Edit.png")+"'/>");
		});
	}
	,onRowClick: function (row) {
		fixedIncomeType.add(row.cell[0]);
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#FixedIncomeTypeList").flexReload();
		}
	}
	,onCreateFixedIncomeTypeBegin: function () {
		$("#UpdateLoading").html(jHelper.savingHTML());
	}
	,onCreateFixedIncomeTypeSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			jAlert(UpdateTargetId.html())
		} else {
			parent.fixedIncomeType.closeDialog(true);
		}
	}
}