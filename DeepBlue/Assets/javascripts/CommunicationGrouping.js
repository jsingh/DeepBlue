var communicationGrouping={
	init: function () {
		jHelper.resizeIframe();
	}
	,add: function (id) {
		var dt=new Date();
		var url="/Admin/EditCommunicationGrouping/"+id+"?t="+dt.getTime();
		jHelper.createDialog(url,{
			title: "Communication Grouping",
			autoOpen: true,
			width: 400,
			modal: true,
			position: 'middle',
			autoResize: true
		});
	}
	,deleteType: function (id,img) {
		if(confirm("Are you sure you want to delete this communication grouping?")) {
			var dt=new Date();
			var url="/Admin/DeleteCommunicationGrouping/"+id+"?t="+dt.getTime();
			$.get(url,function (data) {
				if(data!="") {
					alert(data);
				} else {
					$("#CommunicationGroupingList").flexReload();
				}
			});
		}
	}
	,onSubmit: function (formId) {
		return jHelper.formSubmit(formId);
	}
	,onGridSuccess: function (t) {
		$("tr",t).each(function () {
			$("td:last div",this).html("<img id='Edit' class='gbutton'  src='/Assets/images/Edit.png'/>");
		});
	}
	,onRowClick: function (row) {
		communicationGrouping.add(row.cell[0]);
	}
	,closeDialog: function (reload) {
		$("#addDialog").dialog('close');
		if(reload==true) {
			$("#CommunicationGroupingList").flexReload();
		}
	}
	,onCreateCommunicationGroupingBegin: function () {
		$("#UpdateLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	}
	,onCreateCommunicationGroupingSuccess: function () {
		$("#UpdateLoading").html("");
		var UpdateTargetId=$("#UpdateTargetId");
		if(jQuery.trim(UpdateTargetId.html())!="True") {
			alert(UpdateTargetId.html())
		} else {
			parent.communicationGrouping.closeDialog(true);
		}
	}
}