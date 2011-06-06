dealActivity.findPRCC=function (id,isNew) {
	var dt=new Date();
	var url="/Deal/FindUnderlyingFundPostRecordCapitalCall/"+id+"?t="+dt.getTime();
	$("#PRCCLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...")
	$.getJSON(url,function (data) {
		$("#PRCCLoading").empty();
		var tbl=$("#PRCapitalCallList");
		var bdy=$("tbody",tbl);
		var target;
		var row=$("#UFPRCC_"+id);
		var emptyRow=$("#EmptyUFPRCC_"+id);
		if(row.get(0)) {
			$("#PRCapitalCallAddTemplate").tmpl(data).insertAfter(row);
			row.remove();emptyRow.remove();
		} else {
			row=$("tr:first",bdy);
			$("#PRCapitalCallAddTemplate").tmpl(data).insertBefore(row);
		}
		row=$("#UFPRCC_"+id);
		dealActivity.setUpRow(row);
		if(isNew) { $("#UFPRCC_0",tbl).remove();$("#EmptyUFPRCC_0",tbl).remove(); }
	});
};
dealActivity.makeNewPRCC=function () {
	var ufid=dealActivity.getCCUnderlyingFund();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		alert("Underlying Fund is required");
	} else {
		dealActivity.loadPRCC(false);
	}
};
dealActivity.editPRCC=function (img,id) {
	var tr=$(img).parents("tr:first");
	dealActivity.editRow(tr);
};
dealActivity.addPRCC=function (img,id) {
	var tr=$(img).parents("tr:first");
	var loading=$("#UpdateLoading",tr);
	var isNew=false;
	if(id==0) { isNew=true; }
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	var url="/Deal/CreateUnderlyingFundPostRecordCapitalCall";
	var param=jHelper.serialize(tr);
	$.post(url,param,function (data) {
		loading.empty();
		var arr=data.split("||");
		if(arr[0]=="True") {
			dealActivity.findPRCC(arr[1],isNew);
		} else { alert(data); }
	});
};
dealActivity.deletePRCC=function (id,img) {
	if(confirm("Are you sure you want to delete this underlying fund post record capital call?")) {
		var dt=new Date();
		var url="/Deal/DeleteUnderlyingFundPostRecordCapitalCall/"+id+"?t="+dt.getTime();
		var tr=$(img).parents("tr:first");
		var trid="UFPRCC_"+id;
		var spnloading=$("#UpdateLoading",tr);
		spnloading.html("<img src='/Assets/images/ajax.jpg'/>");
		$.get(url,function (data) {
			if(data!="") {
				alert(data);
			} else {
				spnloading.empty();
				$("input[type='text']",tr).val("");
				tr.addClass("newrow");
				$("#Delete",tr).remove();
			}
		});
	}
};
dealActivity.checkPRCC=function (chk) {
	var box=$("#PRCCListBox");
	if(chk.checked) { box.show(); } else { box.hide(); }
};
dealActivity.loadPRCC=function (isRefresh) {
	var tbl=$("#PRCapitalCallList");
	var loading=$("#PRCCLoading");
	$("#PRCCListBox").show();
	$("#CapitalCall").hide();
	var target=$("tbody",tbl);
	var rowsLength=$("tr",target).length;
	if(rowsLength==0) { isRefresh=true; }
	if(isRefresh) {
		target.empty();
		$("#PRCCListBox").hide();
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON("/Deal/UnderlyingFundPostRecordCapitalCallList",{ "_": (new Date).getTime(),"underlyingFundId": dealActivity.getCCUnderlyingFund() },function (data) {
			loading.empty();
			$.each(data,function (i,item) { $("#PRCapitalCallAddTemplate").tmpl(item).appendTo(target); });
			dealActivity.setUpRow($("tr",target));
			rowsLength=$("tr",target).length;
			if(rowsLength>0) { $("#PRCCListBox").show(); }
		});
	}
};
dealActivity.submitUFPRCapitalCall=function (frm) {
	try {
		var param=$(frm).serializeArray();
		var loading=$("#PRCCLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		param[param.length]={ name: "TotalRows",value: ($("tbody tr","#PRCapitalCallList").length)/2 };
		$.post("/Deal/CreateUnderlyingFundPostRecordCapitalCall",param,function (data) {
			loading.empty();
			if($.trim(data)!="") { alert(data); } else { dealActivity.loadPRCC(true); }
		});
	} catch(e) { alert(e); }
	return false;
};