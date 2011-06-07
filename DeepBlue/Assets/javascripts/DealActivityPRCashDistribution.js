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
	var ufid=dealActivity.getCDUnderlyingFundId();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		alert("Underlying Fund is required");
	} else {
		dealActivity.loadPRCD(false);
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
dealActivity.loadPRCD=function (isRefresh) {
	var loading=$("#PRCDLoading");
	var tbl=$("#PRCashDistributionList");
	$("#PRCDListBox").show();
	$("#CashDistribution").hide();
	var target=$("tbody",tbl);
	var rowsLength=$("tr",target).length;
	if(rowsLength==0) { isRefresh=true; }
	if(isRefresh) {
		target.empty();
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$("#PRCDListBox").hide();
		$.getJSON("/Deal/UnderlyingFundPostRecordCashDistributionList",{ "_": (new Date).getTime(),"underlyingFundId": dealActivity.getCDUnderlyingFundId() },function (data) {
			loading.empty();
			$.each(data,function (i,item) { $("#PRCashDistributionAddTemplate").tmpl(item).appendTo(target); });
			dealActivity.setUpRow($("tr",target));
			rowsLength=$("tr",target).length;
			if(rowsLength>0) { $("#PRCDListBox").show(); }
		});
	}
};
dealActivity.submitUFPRCashDistribution=function (frm) {
	try {
		var param=$(frm).serializeArray();
		var loading=$("#SpnPRCDSaveLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		param[param.length]={ name: "TotalRows",value: ($("tbody tr","#PRCashDistributionList").length)/2 };
		$.post("/Deal/CreateUnderlyingFundPostRecordCashDistribution",param,function (data) {
			loading.empty();
			if($.trim(data)!="") { alert(data); } else { dealActivity.loadPRCD(true); }
		});
	} catch(e) { alert(e); }
	return false;
};