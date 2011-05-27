dealActivity.findPRCD=function (id,isNew) {
	var dt=new Date();
	var url="/Deal/FindUnderlyingFundPostRecordCashDistribution/"+id+"?t="+dt.getTime();
	$("#PRCDLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...")
	$.getJSON(url,function (data) {
		$("#PRCDLoading").empty();
		var tbl=$("#PRCashDistributionList");
		var bdy=$("tbody",tbl);
		var target;
		var row=$("#UFPRCD_"+id);
		var emptyRow=$("#EmptyUFPRCD_"+id);
		if(row.get(0)) {
			$("#PRCashDistributionAddTemplate").tmpl(data).insertAfter(row);
			row.remove();emptyRow.remove();
		} else {
			row=$("tr:first",bdy);
			$("#PRCashDistributionAddTemplate").tmpl(data).insertBefore(row);
		}
		row=$("#UFPRCD_"+id);
		dealActivity.setUpRow(row);
		if(isNew) { $("#UFPRCD_0",tbl).remove();$("#EmptyUFPRCD_0",tbl).remove(); }
	});
};
dealActivity.makeNewPRCD=function () {
	var ufid=dealActivity.getCDUnderlyingFund();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		alert("Underlying Fund is required");
	} else {
		var tbl=$("#PRCashDistributionList");
		var bdy=$("tbody",tbl);
		var newRow=$("#UFPRCD_0",bdy);
		var emptyNewRow=$("#EmptyUFPRCD_0",bdy);
		newRow.remove();
		emptyNewRow.remove();
		var rowsLength=$("tr",bdy).length;
		var data=dealActivity.newPRCDData;
		if(rowsLength>0) {
			var row=$("tr:first",bdy);
			$("#PRCashDistributionAddTemplate").tmpl(data).insertBefore(row);
		} else {
			$("#PRCashDistributionAddTemplate").tmpl(data).appendTo(bdy);
		}
		newRow=$("#UFPRCD_0",bdy);
		$("#UnderlyingFundId",newRow).val(ufid);
		dealActivity.editRow(newRow);
		dealActivity.setUpRow(newRow);
	}
};
dealActivity.editPRCD=function (img,id) {
	var tr=$(img).parents("tr:first");
	dealActivity.editRow(tr);
};
dealActivity.addPRCD=function (img,id) {
	var tr=$(img).parents("tr:first");
	var loading=$("#UpdateLoading",tr);
	var isNew=false;
	if(id==0) { isNew=true; }
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	var url="/Deal/CreateUnderlyingFundPostRecordCashDistribution";
	var param=jHelper.serialize(tr);
	$.post(url,param,function (data) {
		loading.empty();
		var arr=data.split("||");
		if(arr[0]=="True") {
			dealActivity.findPRCD(arr[1],isNew);
		} else { alert(data); }
	});
};
dealActivity.deletePRCD=function (id,img) {
	if(confirm("Are you sure you want to delete this underlying fund post record cash distribution?")) {
		var dt=new Date();
		var url="/Deal/DeleteUnderlyingFundPostRecordCashDistribution/"+id+"?t="+dt.getTime();
		var tr=$(img).parents("tr:first");
		var trid="UFPRCD_"+id;
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
dealActivity.loadPRCD=function () {
	var loading=$("#PRCDLoading");
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
	$.getJSON("/Deal/UnderlyingFundPostRecordCashDistributionList",{ "_": (new Date).getTime(),"underlyingFundId": dealActivity.getCDUnderlyingFund() },function (data) {
		var tbl=$("#PRCashDistributionList");
		loading.empty();
		var target=$("tbody",tbl);
		target.empty();
		$.each(data,function (i,item) { $("#PRCashDistributionAddTemplate").tmpl(item).appendTo(target); });
		dealActivity.setUpRow($("tr",target));
		var rows=$("tr",target).length;
		if(rows>0) {
			$("#PRCDListBox").show();
		}
	});
};