﻿dealActivity.makeNewPRCC=function () {
	var ufid=dealActivity.getCCUnderlyingFund();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		jAlert("Underlying Fund is required");
	} else {
		dealActivity.loadPRCC(false);
	}
};
dealActivity.deletePRCC=function (index,id,img) {
	if(confirm("Are you sure you want to delete this underlying fund post record capital call?")) {
		var dt=new Date();
		var url="/Deal/DeleteUnderlyingFundPostRecordCapitalCall/"+id+"?t="+dt.getTime();
		var tr=$(img).parents("tr:first");
		var trid="UFPRCC_"+id;
		var spnloading=$("#UpdateLoading",tr);
		spnloading.html(jHelper.ajImg());
		$.get(url,function (data) {
			if(data!="") {
				jAlert(data);
			} else {
				spnloading.empty();$("#EmptyUFPRCC_"+index).remove();$("#UFPRCC_"+index).remove();
			}
		});
	}
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
		loading.html(jHelper.loadingHTML());
		$.getJSON("/Deal/UnderlyingFundPostRecordCapitalCallList",{ "_": (new Date).getTime(),"underlyingFundId": dealActivity.getCCUnderlyingFund() },function (data) {
			loading.empty();
			$.each(data,function (i,item) {
				item["Index"]=i;
				$("#PRCapitalCallAddTemplate").tmpl(item).appendTo(target);
			});
			dealActivity.setUpRow($("tr",target));
			rowsLength=$("tr",target).length;
			if(rowsLength>0) { $("#PRCCListBox").show(); }	$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
			$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
		});
	}
};
dealActivity.submitUFPRCapitalCall=function (frm) {
	try {
		var param=$(frm).serializeForm();
		var loading=$("#SpnPRCCSaveLoading");
		loading.html(jHelper.savingHTML());
		param[param.length]={ name: "TotalRows",value: ($("tbody tr","#PRCapitalCallList").length)};
		$.post("/Deal/CreateUnderlyingFundPostRecordCapitalCall",param,function (data) {
			loading.empty();
			if($.trim(data)!="") {
				dealActivity.processErrMsg(data,frm);
			} else {
				jAlert("Post Record Capital Calls Saved");
				//dealActivity.resetCapitalCall();
				//dealActivity.loadPRCC(true); 
			}
		});
	} catch(e) { jAlert(e); }
	return false;
};