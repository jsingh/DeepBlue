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
		dealActivity.loadCC(false);
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
				$("input[type='text']",tr).val("");
				tr.addClass("newrow");
				$("#Delete",tr).remove();
			}
		});
	}
};
dealActivity.setCCUnderlyingFund=function (id,name) {
	$("#CCUnderlyingFundId").val(id);
	$("#SpnCCUFName").html(name);
	$("tbody","#CapitalCallList").empty();
	$("tbody","#PRCapitalCallList").empty();
	dealActivity.loadCC(true);
};
dealActivity.loadCC=function (isRefresh) {
	var tbl=$("#CapitalCallList");
	var loading=$("#CCLoading");
	$("#CapitalCall").show();
	$("#PRCapitalCall").show();
	$("#PRCCListBox").hide();
	var target=$("tbody",tbl);
	var rowsLength=$("tr",target).length;
	if(rowsLength==0) { isRefresh=true; }
	if(isRefresh) {
		target.empty();
		$("#CapitalCall").hide();
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON("/Deal/UnderlyingFundCapitalCallList",{ "_": (new Date).getTime(),"underlyingFundId": dealActivity.getCCUnderlyingFund() },function (data) {
			loading.empty();
			$.each(data,function (i,item) { $("#CapitalCallAddTemplate").tmpl(item).appendTo(target); });
			dealActivity.setUpRow($("tr",target));
			rowsLength=$("tr",target).length;
			if(rowsLength>0) { $("#CapitalCall").show(); }
		});
	}
};
dealActivity.getCCUnderlyingFund=function (id) {
	return parseInt($("#CCUnderlyingFundId").val());
};
dealActivity.submitUFCapitalCall=function (frm) {
	try {
		var param=$(frm).serializeArray();
		var loading=$("#CCLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		param[param.length]={ name: "TotalRows",value: ($("tbody tr","#CapitalCallList").length)/2 };
		$.post("/Deal/CreateUnderlyingFundCapitalCall",param,function (data) {
			loading.empty();
			if($.trim(data)!="") { alert(data); } else { dealActivity.loadCC(true); }
		});
	} catch(e) { alert(e); }
	return false;
};