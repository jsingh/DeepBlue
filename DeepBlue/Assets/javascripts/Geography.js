var geography={
	init: function () {
		jHelper.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditGeography/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: "Geography",
			autoOpen: true,
			width: 380,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,DeleteGeography: function (id,img) {
		if(confirm("Are you sure you want to delete this Geography?")) {
			var dt=new Date();
			var url="/Admin/DeleteGeography/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					jAlert(data);
				} else {
					$("#GeographyList").flexReload();
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
		geography.add(row.cell[0]);
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#GeographyList").flexReload();
		}
	}
	,onGeographyBegin: function () {
		$("#UpdateLoading").html(jHelper.savingHTML());
	}
	,onGeographySuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			jAlert(UpdateTargetId.html())
		} else {
			parent.geography.closeDialog(true);
		}
	}
}