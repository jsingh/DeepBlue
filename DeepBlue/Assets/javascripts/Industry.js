var industry={
	init: function () {
		jHelper.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditIndustry/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: "Industry",
			autoOpen: true,
			width: 380,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,DeleteIndustry: function (id,img) {
		if(confirm("Are you sure you want to delete this Industry?")) {
			var dt=new Date();
			var url="/Admin/DeleteIndustry/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#IndustryList").flexReload();
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
		industry.add(row.cell[0]);
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#IndustryList").flexReload();
		}
	}
	,onIndustryBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onIndustrySuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			alert(UpdateTargetId.html())
		} else {
			parent.industry.closeDialog(true);
		}
	}
}