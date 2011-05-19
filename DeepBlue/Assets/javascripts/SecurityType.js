var securityType={
	init: function () {
		jHelper.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditSecurityType/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: "Security Type",
			autoOpen: true,
			width: 400,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteType: function (id,img) {
		if(confirm("Are you sure you want to delete this security type?")) {
			var dt=new Date();
			var url="/Admin/DeleteSecurityType/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#SecurityTypeList").flexReload();
				}
			});
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId);
	}
	,onGridSuccess: function (t) {
		$("tr",t).each(function () {
			$("td:last div",this).html("<img id='Edit' src='/Assets/images/Edit.gif'/>");
		});
	}
	,onRowClick: function (row) {
		securityType.add(row.cell[0]);
	}
	,onRowBound: function (tr,data) {
		if(data.cell[2]==true) {
			$("td:eq(2) div",tr).html("<img id='Edit' src='/Assets/images/Tick.gif'/>");
		}
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#SecurityTypeList").flexReload();
		}
	}
	,onCreateSecurityTypeBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateSecurityTypeSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			alert(UpdateTargetId.html())
		} else {
			parent.securityType.closeDialog(true);
		}
	}
}