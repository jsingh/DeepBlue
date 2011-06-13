dealActivity.makeNewCD=function () {
	var ufid=dealActivity.getCDUnderlyingFundId();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		alert("Underlying Fund is required");
	} else {
		dealActivity.loadCD(false);
	}
};
dealActivity.deleteCD=function (index,id,img) {
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
				$("#EmptyUFCD_"+index).remove();$("#UFCD_"+index).remove();
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
			$.each(data,function (i,item) { item["Index"]=i;$("#CashDistributionAddTemplate").tmpl(item).appendTo(target); });
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
		var loading=$("#SpnCDSaveLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		param[param.length]={ name: "TotalRows",value: ($("tbody tr","#CashDistributionList").length)/2 };
		$.post("/Deal/CreateUnderlyingFundCashDistribution",param,function (data) {
			loading.empty();
			if($.trim(data)!="") { dealActivity.processErrMsg(data,frm); } else {
				alert("Cash Distribution Saved");
				dealActivity.resetCashDistribution();
			}
		});
	} catch(e) { alert(e); }
	return false;
};
dealActivity.resetCashDistribution=function () {
	$("#CashDistribution").hide();
	$("#PRCashDistribution").hide();
	$("tbody","#CashDistributionList").empty();
	$("#CD_UnderlyingFund").val("");
	$("#SpnCDUFName").empty();
	$("#CDUnderlyingFundId").val(0);
	$("#CD_UnderlyingFund").focus();
};