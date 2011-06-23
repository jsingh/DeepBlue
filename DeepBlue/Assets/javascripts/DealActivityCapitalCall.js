dealActivity.makeNewCC=function () {
	var ufid=dealActivity.getCCUnderlyingFund();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		alert("Underlying Fund is required");
	} else {
		dealActivity.loadCC(false);
	}
};
dealActivity.deleteCC=function (index,id,img) {
	if(confirm("Are you sure you want to delete this underlying fund capital call?")) {
		var dt=new Date();
		var url="/Deal/DeleteUnderlyingFundCapitalCall/"+id+"?t="+dt.getTime();
		var tr=$(img).parents("tr:first");
		var trid="UFCC_"+index;
		var spnloading=$("#UpdateLoading",tr);
		spnloading.html("<img src='/Assets/images/ajax.jpg'/>");
		$.get(url,function (data) {
			if(data!="") {
				alert(data);
			} else {
				spnloading.empty();
				$("#EmptyUFCC_"+index).remove();
				$("#UFCC_"+index).remove();
			}
		});
	}
};
dealActivity.setCCUnderlyingFund=function (id,name) {
	$("#CCUnderlyingFundId").val(id);
	$("#SpnCCUFName").html(name);
	$("tbody","#CapitalCallList").empty();
	$("tbody","#PRCapitalCallList").empty();
	$("#CCDetail").attr("issearch","true").show();
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
		$.getJSON("/Deal/UnderlyingFundCapitalCallList",{ "_": (new Date).getTime()
			,"underlyingFundId": dealActivity.getCCUnderlyingFund()
		},function (data) {
			loading.empty();
			$.each(data,function (i,item) {
				item["Index"]=i;
				$("#CapitalCallAddTemplate").tmpl(item).appendTo(target);
			});
			dealActivity.setUpRow($("tr",target));
			rowsLength=$("tr",target).length;
			if(rowsLength>0) { $("#CapitalCall").show(); }
			$("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
			$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
		});
	}
};
dealActivity.getCCUnderlyingFund=function (id) {
	return parseInt($("#CCUnderlyingFundId").val());
};
dealActivity.submitUFCapitalCall=function (frm) {
	try {
		var param=$(frm).serializeArray();
		var loading=$("#SpnCCSaveLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		param[param.length]={ name: "TotalRows",value: ($("tbody tr","#CapitalCallList").length)};
		$.post("/Deal/CreateUnderlyingFundCapitalCall",param,function (data) {
			loading.empty();
			if($.trim(data)!="") {
				dealActivity.processErrMsg(data,frm);
			} else {
				alert("Capital Calls Saved");
				dealActivity.resetCapitalCall();
			}
		});
	} catch(e) { alert(e); }
	return false;
};
dealActivity.resetCapitalCall=function () {
	$("#CapitalCall").hide();
	$("#PRCapitalCall").hide();
	$("tbody","#CapitalCallList").empty();
	$("#CC_UnderlyingFund").val("");
	$("#SpnCCUFName").empty();
	$("#CCUnderlyingFundId").val(0);
	$("#CC_UnderlyingFund").focus();
};
