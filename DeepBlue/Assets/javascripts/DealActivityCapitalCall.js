dealActivity.findCC=function (id,isEdit) {
	var dt=new Date();
	var url="/Deal/FindUnderlyingFundCapitalCall/"+id+"?t="+dt.getTime();
	var target;
	if(isEdit) { target=$("#EditCC"); } else { target=$("#AddNewCC"); }
	target.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
	$.getJSON(url,function (data) {
		dealActivity.loadTemplate("CapitalCallAddTemplate",target,data);
	});
};
dealActivity.addCC=function (id) {
	var dt=new Date();
	var url="/Deal/EditUnderlyingFundCapitalCall/"+id+"?t="+dt.getTime();
	this.isCapitalCallDialog=true;
	//this.openDialog(url,"Underlying Fund Capital Call");
	var editCC=$("#EditCC");
	editCC.dialog("destroy");
	editCC.dialog({
		title: "Underlying Fund Capital Call",
		autoOpen: true,
		width: 600,
		modal: true,
		position: 'top',
		autoResize: true,
		open: function () {
			dealActivity.findCC(id,true);
			dealActivity.onSuccessSave=function () { editCC.dialog('close'); }
		}
	});
};
dealActivity.deleteCC=function (id,img) {
	if(confirm("Are you sure you want to delete this underlying fund capital call?")) {
		var dt=new Date();
		var url="/Deal/DeleteUnderlyingFundCapitalCall/"+id+"?t="+dt.getTime();
		var tr=$(img).parents("tr:first");
		var spnloading=$("#spnloading",tr);
		spnloading.html("<img src='/Assets/images/ajax.jpg'/>");
		$.get(url,function (data) {
			if(data!="") {
				alert(data);
			} else {
				spnloading.empty();
				var t=$(img).parents("table:first");
				var trid="tr"+id;
				$("#"+trid,t).remove();
				$("#em"+trid,t).remove();
			}
		});
	}
};
dealActivity.onCCSubmit=function (frm) {
	var param=$(frm).serialize();
	var url="/Deal/CreateUnderlyingFundCapitalCall";
	var ULoading=$("#UpdateLoading",frm);
	ULoading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	$.ajax({
		type: "POST",
		url: url,
		data: param,
		success: function (data) {
			ULoading.html("");var arr=data.split("||");
			if(arr[0]!="True") { alert(arr[0]); }
			else {
				jHelper.resetFields(frm);var dt=new Date();
				$.getJSON("/Deal/GetUnderlyingFundCapitalCallList/"+arr[1]+"?t="+dt.getTime(),function (addData) {
					$("#CapitalCallList").ajaxTableAddData(addData);
				});
			} if(dealActivity.onSuccessSave) { dealActivity.onSuccessSave(); }
		},
		error: function (data) { alert(data); }
	});
	return false;
};
dealActivity.setCCUnderlyingFund=function (id) {
	$("#CCUnderlyingFundId").val(id);
	var loading=$("#CCLoading");
	var tbl=$("#CapitalCallList");
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
	loading.empty();
	$.getJSON("/Deal/UnderlyingFundCapitalCallList",{ "_": (new Date).getTime(),"underlyingFundId": id },function (data) {
		$.each(data,function (i,item) {
			dealActivity.loadTemplate("CapitalCallAddTemplate",$("tbody",tbl),item)
		});
	});
};
dealActivity.getCCUnderlyingFund=function (id) {
	return $("#CCUnderlyingFundId").val();
};