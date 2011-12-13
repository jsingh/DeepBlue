dealActivity.makeNewPRDD=function () {
	var ufid=dealActivity.getDDDirect();
	if(isNaN(ufid)) { ufid=0; }
	if(ufid==0) {
		jAlert("Direct is required");
	} else {
		dealActivity.loadPRDD(false);
	}
};
dealActivity.deletePRDD=function (index,id,img) {
	if(confirm("Are you sure you want to delete this post record dividend distribution?")) {
		var dt=new Date();
		var url="/Deal/DeletePostRecordDividendDistribution/"+id+"?t="+dt.getTime();
		var tr=$(img).parents("tr:first");
		var trid="PRDD_"+id;
		var spnloading=$("#UpdateLoading",tr);
		spnloading.html("<img src='/Assets/images/ajax.jpg'/>");
		$.get(url,function (data) {
			if(data!="") {
				jAlert(data);
			} else {
				spnloading.empty();$("#EmptyPRDD_"+index).remove();$("#PRDD_"+index).remove();
			}
		});
	}
};
dealActivity.loadPRDD=function (isRefresh) {
	var tbl=$("#PRDividendDistributionList");
	var loading=$("#PRDDLoading");
	$("#PRDDListBox").show();
	$("#DividendDistribution").hide();
	var target=$("tbody",tbl);
	var rowsLength=$("tr",target).length;
	if(rowsLength==0) { isRefresh=true; }
	if(isRefresh) {
		target.empty();
		$("#PRDDListBox").hide();
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Loading...");
		$.getJSON("/Deal/PostRecordDividendDistributionList"
			,{ "_": (new Date).getTime()
			,"securityTypeID": dealActivity.getDDDirectType()
			,"securityID": dealActivity.getDDDirect()
			},function (data) {
				loading.empty();
				$.each(data,function (i,item) {
					item["Index"]=i;
					$("#PRDividendDistributionAddTemplate").tmpl(item).appendTo(target);
				});
				dealActivity.setUpRow($("tr",target));
				rowsLength=$("tr",target).length;
				if(rowsLength>0) { $("#PRDDListBox").show(); } $("tr:odd",target).removeClass("row").removeClass("arow").addClass("arow");
				$("tr:even",target).removeClass("row").removeClass("arow").addClass("row");
			});
	}
};
dealActivity.submitPRDividendDistribution=function (frm) {
	try {
		var param=$(frm).serializeForm();
		var loading=$("#SpnPRDDSaveLoading");
		loading.html("<img src='/Assets/images/ajax.jpg'/>&nbsp;Saving...");
		param[param.length]={ name: "TotalRows",value: ($("tbody tr","#PRDividendDistributionList").length) };
		$.post("/Deal/CreatePostRecordDividendDistribution",param,function (data) {
			loading.empty();
			if($.trim(data)!="") {
				dealActivity.processErrMsg(data,frm);
			} else {
				jAlert("Post Record Dividend Distributions Saved");
				//dealActivity.resetDividendDistribution();
				//dealActivity.loadPRDD(true); 
			}
		});
	} catch(e) { jAlert(e); }
	return false;
};