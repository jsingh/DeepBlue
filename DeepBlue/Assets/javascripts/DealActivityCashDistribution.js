dealActivity.findCD=function (id,isNew) {
	var dt=new Date();
	var url="/Deal/FindUnderlyingFundCashDistribution/"+id+"?t="+dt.getTime();
	$("#CDLoading").html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...")
	$.getJSON(url,function (data) {
		$("#CDLoading").empty();
		var tbl=$("#CashDistributionList");
		var bdy=$("tbody",tbl);
		var target;
		var row=$("#UFCD_"+id);
		var emptyRow=$("#EmptyUFCD_"+id);
		if(row.get(0)) {
			$("#CashDistributionAddTemplate").tmpl(data).insertAfter(row);
			row.remove();emptyRow.remove();
		} else {
			row=$("tr:first",bdy);
			$("#CashDistributionAddTemplate").tmpl(data).insertBefore(row);
		}
		row=$("#UFCD_"+id);
		dealActivity.setUpRow(row);
		if(isNew) { $("#UFCD_0",tbl).remove();$("#EmptyUFCD_0",tbl).remove(); }
	});
};
dealActivity.makeNewCD=function () {
	var ufid=dealActivity.getCDUnderlyingFundId();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		alert("Underlying Fund is required");
	} else {
		dealActivity.loadCD(false);
	}
};
dealActivity.editCD=function (img,id) {
	var tr=$(img).parents("tr:first");
	dealActivity.editRow(tr);
};
dealActivity.addCD=function (img,id) {
	var tr=$(img).parents("tr:first");
	var loading=$("#UpdateLoading",tr);
	var isNew=false;
	if(id==0) { isNew=true; }
	loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
	var url="/Deal/CreateUnderlyingFundCashDistribution";
	var param=jHelper.serialize(tr);
	$.post(url,param,function (data) {
		loading.empty();
		var arr=data.split("||");
		if(arr[0]=="True") {
			dealActivity.findCD(arr[1],isNew);
		} else { alert(data); }
	});
};
dealActivity.deleteCD=function (id,img) {
	if(confirm("Are you sure you want to delete this underlying fund cash distribution?")) {
		var dt=new Date();
		var url="/Deal/DeleteUnderlyingFundCashDistribution/"+id+"?t="+dt.getTime();
		var tr=$(img).parents("tr:first");
		var trid="UFCD_"+id;
		var spnloading=$("#UpdateLoading",tr);
		spnloading.html("<img src='/Assets/images/ajax.jpg'/>");
		$.get(url,function (data) {
			if(data!="") {
				alert(data);
			} else {
				spnloading.empty();
				$("input[type='text']",tr).val("");
				$("select",tr).val("0");
				tr.addClass("newrow");
				$("#Delete",tr).remove();
			}
		});
	}
};
dealActivity.setCDUnderlyingFund=function (id,name) {
	$("#CDUnderlyingFundId").val(id);
	$("#SpnCDUFName").html(name);
	$("tbody","#CashDistributionList").empty();
	$("tbody","#PRCashDistributionList").empty();
	dealActivity.loadCD(true);
};
dealActivity.loadCD=function (isRefresh) {
	var loading=$("#CDLoading");
	var tbl=$("#CashDistributionList");
	$("#CashDistribution").show();
	$("#PRCDListBox").hide();
	var target=$("tbody",tbl);
	var rowsLength=$("tr",target).length;
	if(rowsLength==0) { isRefresh=true; }
	if(isRefresh) {
		target.empty();
		$("#CashDistribution").hide();
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON("/Deal/UnderlyingFundCashDistributionList",{ "_": (new Date).getTime(),"underlyingFundId": dealActivity.getCDUnderlyingFundId() },function (data) {
			$("#PRCashDistribution").show();
			loading.empty();
			$.each(data,function (i,item) { $("#CashDistributionAddTemplate").tmpl(item).appendTo(target); });
			dealActivity.setUpRow($("tr",target));
			rowsLength=$("tr",target).length;
			if(rowsLength>0) { $("#CashDistribution").show(); }
		});
	}
};
dealActivity.getCDUnderlyingFundId=function (id) {
	return parseInt($("#CDUnderlyingFundId").val());
};
dealActivity.submitUFCashDistribution=function (frm) {
	try {
		var param=$(frm).serializeArray();
		var loading=$("#CDLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		param[param.length]={ name: "TotalRows",value: ($("tbody tr","#CashDistributionList").length)/2 };
		$.post("/Deal/CreateUnderlyingFundCashDistribution",param,function (data) {
			loading.empty();
			if($.trim(data)!="") { alert(data); } else { dealActivity.loadCD(true); }
		});
	} catch(e) { alert(e); }
	return false;
};