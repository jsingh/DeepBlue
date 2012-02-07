dealActivity.makeNewPRCD=function () {
	var ufid=dealActivity.getCDUnderlyingFundId();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		jAlert("Underlying Fund is required");
	} else {
		dealActivity.loadPRCD(false);
	}
};
dealActivity.deletePRCD=function (index,id,img) {
	if(confirm("Are you sure you want to delete this underlying fund post record cash distribution?")) {
		var dt=new Date();
		var url="/Deal/DeleteUnderlyingFundPostRecordCashDistribution/"+id+"?t="+dt.getTime();
		var tr=$(img).parents("tr:first");
		var trid="UFPRCD_"+id;
		var spnloading=$("#UpdateLoading",tr);
		spnloading.html(jHelper.ajImg());
		$.get(url,function (data) {
			if(data!="") {
				jAlert(data);
			} else {
				spnloading.empty();
				$("#EmptyUFPRCD_"+index).remove();
				$("#UFPRCD_"+index).remove();
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
		loading.html(jHelper.loadingHTML());
		$("#PRCDListBox").hide();
		$.getJSON("/Deal/UnderlyingFundPostRecordCashDistributionList",{ "_": (new Date).getTime(),"underlyingFundId": dealActivity.getCDUnderlyingFundId() },function (data) {
			loading.empty();
			$.each(data,function (i,item) { item["Index"]=i;$("#PRCashDistributionAddTemplate").tmpl(item).appendTo(target); });
			dealActivity.setUpRow($("tr",target));
			rowsLength=$("tr",target).length;
			if(rowsLength>0) { $("#PRCDListBox").show(); }	$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
			$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
		});
	}
};
dealActivity.submitUFPRCashDistribution=function (frm) {
	try {
		var param=$(frm).serializeForm();
		var loading=$("#SpnPRCDSaveLoading");
		loading.html(jHelper.savingHTML());
		param[param.length]={ name: "TotalRows",value: ($("tbody tr","#PRCashDistributionList").length)};
		$.post("/Deal/CreateUnderlyingFundPostRecordCashDistribution",param,function (data) {
			loading.empty();
			if($.trim(data)!="") { dealActivity.processErrMsg(data,frm); } else {
				jAlert("Post Record Date Cash Distributions Saved");
				//dealActivity.resetCashDistribution();
			}
		});
	} catch(e) { jAlert(e); }
	return false;
};