dealActivity.findCC=function (id,isNew) {
	var dt=new Date();
	var url="/Deal/FindUnderlyingFundCapitalCall/"+id+"?t="+dt.getTime();
	$("#CCLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...")
	$.getJSON(url,function (data) {
		$("#CCLoading").empty();
		var tbl=$("#CapitalCallList");
		var bdy=$("tbody",tbl);
		var target;
		var row=$("#UFCC_"+id);
		var emptyRow=$("#EmptyUFCC_"+id);
		if(row.get(0)) {
			$("#CapitalCallAddTemplate").tmpl(data).insertAfter(row);
			row.remove();emptyRow.remove();
		} else {
			row=$("tr:first",bdy);
			$("#CapitalCallAddTemplate").tmpl(data).insertBefore(row);
		}
		row=$("#UFCC_"+id);
		dealActivity.setUpRow(row);
		if(isNew) { $("#UFCC_0",tbl).remove();$("#EmptyUFCC_0",tbl).remove(); }
	});
};
dealActivity.makeNewCC=function () {
	var ufid=dealActivity.getCCUnderlyingFund();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		alert("Underlying Fund is required");
	} else {
		var tbl=$("#CapitalCallList");
		var bdy=$("tbody",tbl);
		var newRow=$("#UFCC_0",bdy);
		var emptyNewRow=$("#EmptyUFCC_0",bdy);
		newRow.remove();
		emptyNewRow.remove();
		var rowsLength=$("tr",bdy).length;
		var data=dealActivity.newCCData;
		if(rowsLength>0) {
			var row=$("tr:first",bdy);
			$("#CapitalCallAddTemplate").tmpl(data).insertBefore(row);
		} else {
			$("#CapitalCallAddTemplate").tmpl(data).appendTo(bdy);
		}
		newRow=$("#UFCC_0",bdy);
		$("#UnderlyingFundId",newRow).val(ufid);
		dealActivity.editRow(newRow);
		dealActivity.setUpRow(newRow);
	}
};
dealActivity.editCC=function (img,id) {
	var tr=$(img).parents("tr:first");
	dealActivity.editRow(tr);
};
dealActivity.addCC=function (img,id) {
	var tr=$(img).parents("tr:first");
	var loading=$("#UpdateLoading",tr);
	var isNew=false;
	if(id==0) { isNew=true; }
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	var url="/Deal/CreateUnderlyingFundCapitalCall";
	var param=jHelper.serialize(tr);
	$.post(url,param,function (data) {
		loading.empty();
		var arr=data.split("||");
		if(arr[0]=="True") {
			dealActivity.findCC(arr[1],isNew);
		} else { alert(data); }
	});
};
dealActivity.deleteCC=function (id,img) {
	if(confirm("Are you sure you want to delete this underlying fund capital call?")) {
		var dt=new Date();
		var url="/Deal/DeleteUnderlyingFundCapitalCall/"+id+"?t="+dt.getTime();
		var tr=$(img).parents("tr:first");
		var trid="UFCC_"+id;
		var spnloading=$("#UpdateLoading",tr);
		spnloading.html("<img src='/Assets/images/ajax.jpg'/>");
		$.get(url,function (data) {
			if(data!="") {
				alert(data);
			} else {
				spnloading.empty();
				$("#"+trid).remove();
				$("#Empty"+trid).remove();
			}
		});
	}
};
dealActivity.setCCUnderlyingFund=function (id,name) {
	$("#CCUnderlyingFundId").val(id);
	var loading=$("#CCLoading");
	var tbl=$("#CapitalCallList");
	$("#SpnCCUFName").html(name);
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
	$("#PRCapitalCall").hide();
	$.getJSON("/Deal/UnderlyingFundCapitalCallList",{ "_": (new Date).getTime(),"underlyingFundId": id },function (data) {
		$("#PRCapitalCall").show();
		loading.empty();
		var target=$("tbody",tbl);
		target.empty();
		$.each(data,function (i,item) { $("#CapitalCallAddTemplate").tmpl(item).appendTo(target); });
		dealActivity.setUpRow($("tr",target));
		dealActivity.loadPRCC();
	});
};
dealActivity.getCCUnderlyingFund=function (id) {
	return parseInt($("#CCUnderlyingFundId").val());
};