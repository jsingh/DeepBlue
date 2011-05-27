dealActivity.findUFV=function (id,isNew) {
	var dt=new Date();
	var url="/Deal/FindUnderlyingFundValuation/"+id+"?t="+dt.getTime();
	$("#UFVLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...")
	$.getJSON(url,function (data) {
		$("#UFVLoading").empty();
		var tbl=$("#UnderlyingFundValuationList");
		var bdy=$("tbody",tbl);
		var target;
		var row=$("#UFV_"+id);
		var emptyRow=$("#EmptyUFV_"+id);
		if(row.get(0)) {
			$("#UFValuationAddTemplate").tmpl(data).insertAfter(row);
			row.remove();emptyRow.remove();
		} else {
			row=$("tr:first",bdy);
			$("#UFValuationAddTemplate").tmpl(data).insertBefore(row);
		}
		row=$("#UFV_"+id);
		dealActivity.setUpRow(row);
		if(isNew) { $("#UFV_0",tbl).remove();$("#EmptyUFV_0",tbl).remove(); }
	});
};
dealActivity.makeNewUFV=function () {
	var ufid=dealActivity.getUFVUnderlyingFund();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		alert("Underlying Fund is required");
	} else {
		var tbl=$("#UnderlyingFundValuationList");
		var bdy=$("tbody",tbl);
		var newRow=$("#UFV_0",bdy);
		var emptyNewRow=$("#EmptyUFV_0",bdy);
		newRow.remove();
		emptyNewRow.remove();
		var rowsLength=$("tr",bdy).length;
		var data=dealActivity.newUFVData;
		if(rowsLength>0) {
			var row=$("tr:first",bdy);
			$("#UFValuationAddTemplate").tmpl(data).insertBefore(row);
		} else {
			$("#UFValuationAddTemplate").tmpl(data).appendTo(bdy);
		}
		newRow=$("#UFV_0",bdy);
		dealActivity.editRow(newRow);
		dealActivity.setUpRow(newRow);
	}
};
dealActivity.editUFV=function (img,id) {
	var tr=$(img).parents("tr:first");
	dealActivity.editRow(tr);
};
dealActivity.addUFV=function (img,id) {
	var tr=$(img).parents("tr:first");
	var loading=$("#UpdateLoading",tr);
	var isNew=false;
	if(id==0) { isNew=true; }
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	var url="/Deal/CreateUnderlyingFundValuation";
	var param=jHelper.serialize(tr);
	param[param.length]={ name: "UnderlyingFundId",value: dealActivity.getUFVUnderlyingFund() };
	$.post(url,param,function (data) {
		loading.empty();
		var arr=data.split("||");
		if(arr[0]=="True") {
			dealActivity.findUFV(arr[1],isNew);
		} else { alert(data); }
	});
};
dealActivity.deleteUFV=function (id,img) {
	if(confirm("Are you sure you want to delete this underlying fund valuation?")) {
		var dt=new Date();
		var url="/Deal/DeleteUnderlyingFundValuation/"+id+"?t="+dt.getTime();
		var tr=$(img).parents("tr:first");
		var trid="UFV_"+id;
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
dealActivity.setUFVUnderlyingFund=function (id,name) {
	$("#UFVUnderlyingFundId").val(id);
	var loading=$("#UFVLoading");
	var tbl=$("#UnderlyingFundValuationList");
	$("#SpnUFVName").html(name);
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
	$("#PRValuation").hide();
	$.getJSON("/Deal/UnderlyingFundValuationList",{ "_": (new Date).getTime(),"underlyingFundId": id },function (data) {
		$("#PRValuation").show();
		loading.empty();
		var target=$("tbody",tbl);
		target.empty();
		$.each(data,function (i,item) { $("#UFValuationAddTemplate").tmpl(item).appendTo(target); });
		dealActivity.setUpRow($("tr",target));
	});
};
dealActivity.getUFVUnderlyingFund=function (id) {
	return parseInt($("#UFVUnderlyingFundId").val());
};