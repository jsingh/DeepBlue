var purchaseType={
	init: function () {
		jHelper.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditPurchaseType/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: "Purchase Type",
			autoOpen: true,
			width: 400,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteType: function (id,img) {
		if(confirm("Are you sure you want to delete this purchase type?")) {
			var dt=new Date();
			var url="/Admin/DeletePurchaseType/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#PurchaseTypeList").flexReload();
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
		purchaseType.add(row.cell[0]);
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#PurchaseTypeList").flexReload();
		}
	}
	,onCreatePurchaseTypeBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreatePurchaseTypeSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			alert(UpdateTargetId.html())
		} else {
			parent.purchaseType.closeDialog(true);
		}
	}
}