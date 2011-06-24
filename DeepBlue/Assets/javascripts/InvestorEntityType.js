var invEntityType={
	init: function () {
		jHelper.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditInvestorEntityType/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{ title: "Investor Entity Type",
			autoOpen: true,
			width: 400,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteEntityType: function (id,img) {
		if(confirm("Are you sure you want to delete this investor entity type?")) {
			var dt=new Date();
			var url="/Admin/DeleteInvestorEntityType/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#InvEntityTypeList").flexReload();
				}
			});
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId);
	}
	,onGridSuccess: function (t) {
		$("tr",t).each(function () {
			$("td:last div",this).html("<img src='/Assets/images/Edit.png'/>");
		});
	}
	,onRowClick: function (row) {
		invEntityType.add(row.cell[0]);
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#InvEntityTypeList").flexReload();
		}
	}
	,onCreateInvEnityTypeBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateInvEnityTypeSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			alert(UpdateTargetId.html())
		} else {
			parent.invEntityType.closeDialog(true);
		}
	}
}